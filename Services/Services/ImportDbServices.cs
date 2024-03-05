using System.Text.Json;
using System.Text.RegularExpressions;
using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;
using IdentityServer4.Extensions;
using OneOf;
using Services.Dtos;

namespace Services.Services;

public class ImportDbServices : CustomServiceBase
{
    public ImportDbServices(EntityDbContext context) : base(context)
    {
    }
    public async Task<OneOf<ResponseErrorDto, string>> ImportData(
        int organization, string descriptionOfSurvey, string filePath)
    {
        //string filePath = "C:/Users/Diego/kEncuesta.json";
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

        }

        foreach (var dictionaryValue in keyValuePairs)
        {
            string surveyAsk = string.Empty;
            List<string> responsePosibilities = new List<string>();
            string suerveyAskResponse = string.Empty;
            for (int i = 0; i < dictionaryValue.Count(); i++)
            {
                string value = $"{dictionaryValue.ElementAt(i).Key}: {dictionaryValue.ElementAt(i).Value}";
                string pattern = @"^(.*?\?) \[(.*?)\]: (.*)$";

                Regex regex = new Regex(pattern);
                Match match = regex.Match(value);
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
                }
                else if (!surveyAsk.Equals(match.Groups[1].Value.Trim()) || i == dictionaryValue.Count() - 1)
                {
                    var newSurvey = new Survey()
                    {
                        Description = descriptionOfSurvey,
                        SatiscationState = "",
                        OrganizationId = organization,
                    };
                    _context.Surveys.Add(newSurvey);
                   
                    var newSurveyAsk = new SurveyAsk()
                    {
                        Description = surveyAsk,
                        Survey = newSurvey,
                        
                    };
                    _context.SurveyAsks.Add(newSurveyAsk);
                    
                    int counterId = 0;
                    foreach (var response in responsePosibilities)
                    {
                        _context.ResponsePosibilities.Add(new ResponsePosibility()
                        {
                            //SuveryAskId = surveyAskId!.Id,
                            SurveyAsk = newSurveyAsk,
                            ResponseValue = response,
                            Id = counterId
                        });
                        counterId++;
                    }

                    await _context.SaveChangesAsync();
                    
                    var findSurvey =_context.Surveys.OrderBy(x=>x.Id).LastOrDefault();
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
                    await _context.SaveChangesAsync();
                    surveyAsk = string.Empty;
                    responsePosibilities = new List<string>();
                    suerveyAskResponse = string.Empty;
                }
                responsePosibilities.Add(match.Groups[2].Value.Trim());
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
    public async Task<OneOf<ResponseErrorDto, string>> FindObject()
    {
        var file = "./../Services/Info/";
        var surveys = new List<Survey>();
        var filesPending = new Stack<string>();

        // Agregar la carpeta principal a la pila
        filesPending.Push(file);

        while (filesPending.Count > 0)
        {
            string actualFile = filesPending.Pop();
            string fileName = Path.GetFileName(actualFile);

            // Obtener la lista de archivos JSON en la carpeta actual
            string[] jsonFiles = Directory.GetFiles(actualFile, "*.json");

            // Iterar sobre cada archivo JSON
            foreach (var jsonFile in jsonFiles)
            {
                try
                {
                    
                    var organization = _context.Organization.FirstOrDefault(x=>x.Name == fileName);
                    if (organization is null)
                    {
                        return new ResponseErrorDto()
                        {
                            ErrorCode = 404,
                            ErrorMessage = "Organization not found"
                        };
                    }
                    //var importData = await ImportData(organization.Id, Path.GetFileNameWithoutExtension(jsonFile), Path.GetFullPath(jsonFile));
                    var importData = await ImportData2(organization.Id, Path.GetFileNameWithoutExtension(jsonFile), Path.GetFullPath(jsonFile));
                    if (importData.TryPickT0(out var error, out var message))
                    {
                        return error;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al procesar el archivo {jsonFile}: {ex.Message}");
                }
            }

            // Agregar las subcarpetas a la pila
            string[] subcarpetas = Directory.GetDirectories(actualFile);
            foreach (var subcarpeta in subcarpetas)
            {
                filesPending.Push(subcarpeta);
            }
        }

        return "ok";
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

    
    
    
    
    
    
    
    
    public async Task<OneOf<ResponseErrorDto, string>> ImportData2(
        int organization, string descriptionOfSurvey, string filePath)
    {
        //string filePath = "C:/Users/Diego/kEncuesta.json";
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

        }
        var newSurvey = new Survey()
        {
            Description = descriptionOfSurvey,
            SatiscationState = "",
            OrganizationId = organization,
            SurveyAsks = new List<SurveyAsk>(),
            
        };
        var count = 0;
        
        //agregar survey
        string surveyAsk = string.Empty;
        List<string> responsePosibilities = new List<string>();
        for (int i = 0; i < keyValuePairs.ElementAt(0).Count(); i++)
        {
            string value = $"{keyValuePairs.ElementAt(0).ElementAt(i).Key}: {keyValuePairs.ElementAt(0).ElementAt(i).Value}";
            string pattern = @"^(.*?\?) \[(.*?)\]: (.*)$";

            Regex regex = new Regex(pattern);
            Match match = regex.Match(value);
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
            }
            else if (!surveyAsk.Equals(match.Groups[1].Value.Trim()) || i == keyValuePairs.ElementAt(0).Count() - 1)
            {
                var newSurveyAsk = new SurveyAsk()
                {
                    Description = surveyAsk,
                    Survey = newSurvey,
                    ResponsePosibilities = new List<ResponsePosibility>(),
                    SurveyResponses = new List<SurveyResponse>()
                        
                };
                newSurvey.SurveyAsks.Add(newSurveyAsk);
                    
                foreach (var response in responsePosibilities)
                {
                    newSurveyAsk.ResponsePosibilities.Add(new ResponsePosibility()
                    {
                        SurveyAsk = newSurveyAsk,
                        ResponseValue = response,
                        Id = count
                    });
                    count++;
                }

                surveyAsk = null;
            }
            responsePosibilities.Add(match.Groups[2].Value.Trim());
        }

        _context.Surveys.Add(newSurvey);
        
        //agregar respuestas

        #region MyRegion

        foreach (var dictionaryValue in keyValuePairs)
        {
            surveyAsk = string.Empty;
            string suerveyAskResponse = string.Empty;
            for (int i = 0; i < dictionaryValue.Count(); i++)
            {
                string value = $"{dictionaryValue.ElementAt(i).Key}: {dictionaryValue.ElementAt(i).Value}";
                string pattern = @"^(.*?\?) \[(.*?)\]: (.*)$";

                Regex regex = new Regex(pattern);
                Match match = regex.Match(value);
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
                }
                
                if (match.Groups[3].Value.Trim().ToLower().Equals("sí"))
                {
                    suerveyAskResponse = match.Groups[2].Value.Trim();
                    var ask = newSurvey.SurveyAsks.FirstOrDefault(x => x.Description == surveyAsk);
                    var response = new SurveyResponse()
                    {
                        SurveyAsk = ask,
                        ResponsePosibility = ask.ResponsePosibilities
                            .FirstOrDefault(x=>x.ResponseValue.Equals(suerveyAskResponse))
                    };
                    _context.SurveyResponses.Add(response);
                }
            }
        }

        #endregion
        await _context.SaveChangesAsync();
        return "Import sussed";
    }
}