using ExpenseReportCSharp.Adapter;

namespace Tests;

public class FakeSystemOutProvider : SystemOutProvider {
    private List<string> messages = new List<string>();

    public override void ServicePrint(String message) {
        this.messages.Add(message);
    }
    public List<String> Messages() {
        return messages;
    }
    
}