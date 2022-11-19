namespace EchoExtension;

public partial record Echo(string Message) : ICommand<string>;

[Shared]
partial record class EchoHandler : ICommandHandler<Echo, string>
{
    public bool CanExecute(Echo command) => !string.IsNullOrEmpty(command.Message);

    public string Execute(Echo command)
        => string.IsNullOrEmpty(command.Message) ?
            throw new NotSupportedException("Cannot echo an empty or null message") :
            command.Message;
}

static partial class UI
{
    [MenuCommand("File.E_cho")]
    public static async Task RunAsync(IMessageBus bus, IThreadingContext threading, CancellationToken cancellation)
    {
        await threading.SwitchToForeground(cancellation);
        
        if (MessageBox.Query("Echo", "Do you want to echo 'Hello World'?", "Obviously", "Nah") == 0)
            MessageBox.Query("Echo", "We got back: " + bus.Execute(new Echo("Hello World")), "Ok");
    }
}