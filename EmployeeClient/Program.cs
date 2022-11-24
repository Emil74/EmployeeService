using Grpc.Net.Client;
using static EmployeeServiceProto.DictionariesService;

namespace EmployeeClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var chanel = GrpcChannel.ForAddress("https://localhost:5001");
            DictionariesServiceClient client = new DictionariesServiceClient(chanel);

            Console.Write("Укажите тип сотрудника: ");

            var response = client.CreateEmployeeType(new EmployeeServiceProto.CreateEmployeeTypeRequest
            {
                Description = Console.ReadLine()
            });

            if (response != null)
            {
                Console.WriteLine($"Тип сотрудника успешно добавлен: #{response.Id}");
            }
            var getAllEmployeeTypesResponse = client.GetAllEmployeeTypes(new EmployeeServiceProto.GetAllEmployeeTypesRequest());
            foreach (var employeeType in getAllEmployeeTypesResponse.EmployeeTypes)
            {
                Console.WriteLine($"#{employeeType.Id} / {employeeType.Description}");
            }
            Console.ReadKey(true);
        }
    }
}