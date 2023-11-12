using ExpenseReportCSharp.Adapter;

namespace Tests;

public class FakeSystemOutProvider : SystemOutProvider {
    private readonly List<string> messages = new();

    public override void ServicePrint(String message) {
        this.messages.Add(message);
    }
    public List<String> Messages() {
        return messages;
    }
}