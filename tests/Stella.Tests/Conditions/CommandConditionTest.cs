using FluentAssertions;
using JetBrains.Annotations;
using Stella.Conditions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Stella.Tests.Conditions;

[TestSubject(typeof(CommandCondition))]
public class CommandConditionTest
{

    [Theory]
    [InlineData("start", "/start", true)]
    [InlineData("start", "/starting", false)]
    [InlineData("start", "start", false)]
    [InlineData("start", "/start@bot", true)]
    public async Task CheckAsync_CorrectCommandParsing(string expectedCommand, string messageText, bool result)
    {
        // assign
        var update = new Update
        {
            Message = new Message
            {
                Text = messageText,
                Entities = [new MessageEntity
                {
                    Type = MessageEntityType.BotCommand,
                    Offset = 0,
                    Length = messageText.Length
                }]
            }
        };
        var condition = new CommandCondition();

        // act
        var checkResult = await condition.CheckAsync(update, expectedCommand);
        
        // assert
        checkResult.Should().Be(result);
    }
}