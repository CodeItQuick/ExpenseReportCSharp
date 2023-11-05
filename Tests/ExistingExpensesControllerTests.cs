 using System.Net;

 namespace Tests;

 public class ExistingExpensesControllerTests : IClassFixture<TestingWebAppFactory<Program>>
 {
     private readonly HttpClient client;

     public ExistingExpensesControllerTests(TestingWebAppFactory<Program> factory)
     {
         client = factory.CreateClient();
     }

     [Fact]
     public async Task CanConstructDefaultExpenseServiceAndViewExpenses()
     {
         var response = await client.GetAsync($"/Home/Index");
         
         Assert.Equal(HttpStatusCode.OK, response.StatusCode);
     }
 }