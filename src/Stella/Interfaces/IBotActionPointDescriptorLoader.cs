using System.Reflection;

namespace Stella.Interfaces;

public interface IBotActionPointDescriptorLoader
{
    BotActionPointDescriptor Load(MemberInfo memberInfo);
}