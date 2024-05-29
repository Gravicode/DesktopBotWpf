using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DesktopBotWpf;

internal class PluginInUseMessage(Dictionary<string, string> value) : ValueChangedMessage<Dictionary<string, string>>(value);

internal class KernelErrorMessage(string value) : ValueChangedMessage<string>(value);
