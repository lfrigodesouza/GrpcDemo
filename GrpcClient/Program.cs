using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            // var input = new HelloRequest {Name = "Lucas"};
            // var channel = GrpcChannel.ForAddress("https://localhost:5001");
            // var client = new Greeter.GreeterClient(channel);
            //
            // var reply = await client.SayHelloAsync(input);
            //
            // Console.WriteLine(reply.Message);

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var customerClient = new Customer.CustomerClient(channel);

            var request = new CustomerLookupModel{ UserId = 1 };

            var customer = await customerClient.GetCustomerInfoAsync(request);

            Console.WriteLine($"{customer.FirstName} {customer.LastName}");
            Console.WriteLine();
            Console.WriteLine("New Customers List");
            Console.WriteLine();

            using (var call = customerClient.GetNewCustomers(new NewCustomerRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"{currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.EmailAddress}");
                }
            }

            Console.ReadLine();
        }
    }
}
