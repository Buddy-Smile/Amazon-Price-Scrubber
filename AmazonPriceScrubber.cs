using System;
using System.Net.Http;
using HtmlAgilityPack;

class Program
{
    static void Main()
    {
        Console.Write("Enter the URL of the product page: ");
        string url = Console.ReadLine();

        string price = GetProductPrice(url);

        Console.WriteLine($"The product price is: {price}");
    }

    static string GetProductPrice(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string htmlContent = response.Content.ReadAsStringAsync().Result;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // Locate the span containing "Price:"
                HtmlNode priceLabelNode = doc.DocumentNode.SelectSingleNode("//span[contains(text(),'Price:')]");

                if (priceLabelNode != null)
                {
                    // Get the next sibling node (assumed to contain the actual price)
                    HtmlNode priceNode = priceLabelNode.NextSibling;

                    if (priceNode != null)
                    {
                        string price = priceNode.InnerText.Trim();
                        return price;
                    }
                    else
                    {
                        return "Price element not found after the label.";
                    }
                }
                else
                {
                    return "Price label not found on the page.";
                }
            }
            else
            {
                return $"Failed to retrieve the page. Status code: {response.StatusCode}";
            }
        }
    }
}

