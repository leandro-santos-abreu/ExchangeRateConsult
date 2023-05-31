using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trabalho1
{
    public class Program
    {
        private static string baseUrl = "https://v6.exchangerate-api.com/v6/8d37ddc36d84e0c79d6395cc/latest/";
        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            while (true)
            {
                Console.WriteLine("\nEscolha a Moeda para Exibir as Taxas de Câmbio:\n");
                Console.WriteLine("1 - USD");
                Console.WriteLine("2 - EUR");
                Console.WriteLine("3 - JPY");
                Console.WriteLine("4 - Outra Moeda");
                Console.WriteLine("0 - Sair");

                Console.Write("\nEscolha: ");
                var escolha = Console.ReadLine();
                switch (escolha)
                {
                    case "1":
                        var responseUSD = await client.GetAsync(GetCurrencyUrl("USD"));
                        var dtoUSD = JsonConvert.DeserializeObject<ExchangeRateDto>(responseUSD.Content.ReadAsStringAsync().Result);

                        ShowRates(dtoUSD);

                        break;
                    case "2":
                        var responseEUR = await client.GetAsync(GetCurrencyUrl("EUR"));
                        var dtoEUR = JsonConvert.DeserializeObject<ExchangeRateDto>(responseEUR.Content.ReadAsStringAsync().Result);

                        ShowRates(dtoEUR);

                        break;
                    case "3":
                        var responseJPY = await client.GetAsync(GetCurrencyUrl("JPY"));
                        var dtoJPY = JsonConvert.DeserializeObject<ExchangeRateDto>(responseJPY.Content.ReadAsStringAsync().Result);

                        ShowRates(dtoJPY);


                        break;
                    case "4":
                        Console.WriteLine("Digite a Abreviação da Moeda Desejada:");
                        var currency = Console.ReadLine();

                        var genericResponse = await client.GetAsync(GetCurrencyUrl(currency));
                        var genericDto = JsonConvert.DeserializeObject<ExchangeRateDto>(genericResponse.Content.ReadAsStringAsync().Result);

                        ShowRates(genericDto);

                        break;
                     default:
                        break;
                }
                if (escolha == "0")
                {
                    Console.WriteLine("Saindo...");
                    await Task.Delay(1000);
                    break;
                }
            }


        }

        private static string GetCurrencyUrl(string currecy)
        {
            return baseUrl + currecy;
        }

        private static void ShowRates(ExchangeRateDto dto)
        {
            Console.WriteLine($"\nData de Última Atualização: {dto.time_last_update_utc}\n");
            if (dto.conversion_rates == null)
                Console.WriteLine("Não foi Encontrada a Moeda Desejada.");

            foreach (var rate in dto.conversion_rates)
            {
                Console.WriteLine($"{rate.Key}: {rate.Key} {rate.Value.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"))}");
            }
        }

        private class ExchangeRateDto
        {
            public string result;
            public string documentation;
            public string terms_of_use;
            public int time_last_update_unix;
            public string time_last_update_utc;
            public int time_next_update_unix;
            public string time_next_update_utc;
            public string base_code;
            public Dictionary<string, float> conversion_rates;

        }
    }
}
