using System.Text.Json;
using System.Text.RegularExpressions;
using BePrácticasLaborales.DataAcces;
using BePrácticasLaborales.Dtos;
using IdentityServer4.Extensions;
using OneOf;

namespace BePrácticasLaborales.Services;

public class ImportDbServices : CustomServiceBase
{
    public ImportDbServices(EntityDbContext context) : base(context)
    {
    }
    public async Task<OneOf<ResponseErrorDto, string>> ImportData(int organization)
    {
        string filePath = "C:/Users/Diego/kEncuesta.json";
        string fileInfo = string.Empty;
        try
        {
            using (StreamReader fileReader = File.OpenText(filePath))
            {
                while (!fileReader.EndOfStream)
                {
                    fileInfo = fileReader.ReadToEnd();
                    //System.Console.WriteLine(test);
                }
            }
        }
        catch (IOException ex)
        {
            //System.Console.WriteLine(ex.Message);
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Error reading the file"
            };
        }

        ICollection<Dictionary<string, string>> keyValuePairs = new List<Dictionary<string, string>>();

        try
        {
            using (JsonDocument jsonDocument = JsonDocument.Parse(fileInfo))
            {
                // Acceder al array "responses" en el JSON
                JsonElement responsesArray = jsonDocument.RootElement.GetProperty("responses");
                Dictionary<string, string> helpDictionary = new Dictionary<string, string>();
                // Iterar sobre cada elemento del array "responses"
                foreach (JsonElement responseElement in responsesArray.EnumerateArray())
                {
                    ExploreJsonElement(responseElement, helpDictionary);
                    keyValuePairs.Add(helpDictionary);
                }
            }
        }
        catch (JsonException ex)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Error al analizar el JSON: {ex.Message}"
            };
            //Console.WriteLine($"Error al analizar el JSON: {ex.Message}");
        }

        foreach (var dictionaryValue in keyValuePairs)
        {
            string surveyAsk = string.Empty;
            List<string> responsePosibilities = new List<string>();
            string suerveyAskResponse = string.Empty;
            foreach (var kvp in dictionaryValue)
            {
                string a = $"{kvp.Key}: {kvp.Value}";
                string pattern = @"^(.*?\?) \[(.*?)\]: (.*)$";

                Regex regex = new Regex(pattern);
                Match match = regex.Match(a);
                if (!match.Success)
                {
                    return new ResponseErrorDto()
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Match data is fail"
                    };
                }

                if (surveyAsk.IsNullOrEmpty())
                {
                    surveyAsk = match.Groups[1].Value.Trim();
                    responsePosibilities.Add(match.Groups[2].Value.Trim());
                }
                else
                {
                    var newSurvey = new Survey()
                    {
                        //todo: arreglar la descipcion
                        Description = "no se que descipcion ponerle",
                        SatiscationState = "no se que poner toampoco",
                        OrganizationId = organization,
                    };
                    _context.Surveys.Add(newSurvey);
                    
                    var newSurveyAsk = new SurveyAsk()
                    {
                        Description = surveyAsk,
                        Survey = newSurvey,
                        
                    };
                    _context.SurveyAsks.Add(newSurveyAsk);
                    foreach (var response in responsePosibilities)
                    {
                        _context.ResponsePosibilities.Add(new ResponsePosibility()
                        {
                            SurveyAsk = newSurveyAsk,
                            ResponseValue = response
                        });
                    }

                    var findSurvey =_context.Surveys.LastOrDefault();
                    var findSurveyAsk = _context.SurveyAsks.FirstOrDefault(x=> x.SurveyId == findSurvey!.Id
                                                                               && x.Description.Equals(surveyAsk));
                    var surveyResponse = _context.ResponsePosibilities.FirstOrDefault(x =>
                        x.SuveryAskId == findSurveyAsk!.Id
                        && x.ResponseValue == suerveyAskResponse);
                    var newSurveyResponse = new SurveyResponse()
                    {
                        SuveryAskId = 0,
                        ResponsePosibilityId = surveyResponse!.Id
                    };
                    _context.SurveyResponses.Add(newSurveyResponse);
                    //await _context.SaveChangesAsync();
                    surveyAsk = string.Empty;
                    responsePosibilities = new List<string>();
                    suerveyAskResponse = string.Empty;
                }

                if (match.Groups[3].Value.Trim().ToLower().Equals("sí"))
                {
                    suerveyAskResponse = match.Groups[2].Value.Trim();
                }
            }
        }
        await _context.SaveChangesAsync();
        return "Import sussed";
    }


    
    static void ExploreJsonElement(JsonElement jsonElement, Dictionary<string, string> keyValuePairs, string currentPath = "")
    {
        if (jsonElement.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in jsonElement.EnumerateObject())
            {
                string propertyName = property.Name;
                string propertyPath = string.IsNullOrEmpty(currentPath) ? propertyName : $"{currentPath}.{propertyName}";

                ExploreJsonElement(property.Value, keyValuePairs, propertyPath);
            }
        }
        else
        {
            keyValuePairs[currentPath] = jsonElement.ToString();
        }
    }

    //todo: creo que esto lo puedo eliminar 
    static Dictionary<string, string> ParseJsonToDictionary(string jsonString)
    {
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        try
        {
            using (JsonDocument jsonDocument = JsonDocument.Parse(jsonString))
            {
                ExploreJsonElement(jsonDocument.RootElement, keyValuePairs);
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error al analizar el JSON: {ex.Message}");
        }

        return keyValuePairs;
    }

}