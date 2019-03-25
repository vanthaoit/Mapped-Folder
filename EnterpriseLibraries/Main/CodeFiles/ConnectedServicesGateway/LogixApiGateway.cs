namespace LogixHealth.EnterpriseLibrary.AppServices.Gateway
{
    public interface ILogixApiGateway
    {
        System.Threading.Tasks.Task<ResponseModel<T>> Get<T>(string routes);

        System.Threading.Tasks.Task<ResponseModel<R>> Post<T, R>(string routes, dynamic payLoad);

        System.Threading.Tasks.Task<ResponseModel<R>> Put<T, R>(string routes, dynamic payLoad);

        System.Threading.Tasks.Task<ResponseModel<T>> Delete<T>(string routes, dynamic payLoad);
    }

    public class LogixApiGateway : ILogixApiGateway
    {
        private System.Net.Http.HttpClient ApiClient { get; set; }

        public LogixApiGateway(System.Net.Http.HttpClient client)
        {
            client.AddLogixRequestHeaders();
            ApiClient = client;
        }

        public System.Threading.Tasks.Task<ResponseModel<T>> Delete<T>(string routes, dynamic payLoad)
        {
            try
            {
                return ServerResponseAsync<T>(ApiClient.DeleteAsync(routes).Result);
            }
            catch (System.Net.Http.HttpRequestException exception)
            {
                return System.Threading.Tasks.Task<ResponseModel<T>>
                    .Factory
                    .StartNew
                    (
                        () =>
                            new ResponseModel<T>
                            {
                                IsSuccess = false,
                                FaultMessageOnFailure = new ResponseException
                                {
                                    ApiErrorMessage = exception.Message,
                                    ErrorCode = System.Net.HttpStatusCode.InternalServerError,
                                    StackTrace = exception.StackTrace
                                },
                                Data = default(T)
                            }
                    );
            }
            catch (System.Exception exception)
            {
                return System.Threading.Tasks.Task<ResponseModel<T>>
                    .Factory
                    .StartNew
                    (
                        () =>
                            new ResponseModel<T>
                            {
                                IsSuccess = false,
                                FaultMessageOnFailure = new ResponseException
                                {
                                    ApiErrorMessage = exception.Message,
                                    ErrorCode = System.Net.HttpStatusCode.InternalServerError,
                                    StackTrace = exception.StackTrace
                                },
                                Data = default(T)
                            }
                    );
            }
        }

        public System.Threading.Tasks.Task<ResponseModel<T>> Get<T>(string routes)
        {
            try
            {
                return ServerResponseAsync<T>(ApiClient.GetAsync(routes).Result);
            }
            catch (System.Net.Http.HttpRequestException exception)
            {
                return System.Threading.Tasks.Task<ResponseModel<T>>
                    .Factory
                    .StartNew
                    (
                        () =>
                            new ResponseModel<T>
                            {
                                IsSuccess = false,
                                FaultMessageOnFailure = new ResponseException
                                {
                                    ApiErrorMessage = exception.Message,
                                    ErrorCode = System.Net.HttpStatusCode.InternalServerError,
                                    StackTrace = exception.StackTrace
                                },
                                Data = default(T)
                            }
                    );
            }
            catch (System.Exception exception)
            {
                return System.Threading.Tasks.Task<ResponseModel<T>>
                    .Factory
                    .StartNew
                    (
                        () =>
                            new ResponseModel<T>
                            {
                                IsSuccess = false,
                                FaultMessageOnFailure = new ResponseException
                                {
                                    ApiErrorMessage = exception.Message,
                                    ErrorCode = System.Net.HttpStatusCode.InternalServerError,
                                    StackTrace = exception.StackTrace
                                },
                                Data = default(T)
                            }
                    );
            }
        }

        public System.Threading.Tasks.Task<ResponseModel<R>> Post<T, R>(string routes, dynamic payLoad)
        {
            try
            {
                return ServerResponseAsync<R>(ApiClient.PostAsync
                    (
                        routes,
                        new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payLoad),
                        System.Text.Encoding.UTF8, "application/json")
                    ).Result);
            }
            catch (System.Net.Http.HttpRequestException exception)
            {
                return System.Threading.Tasks.Task<ResponseModel<R>>
                    .Factory
                    .StartNew
                    (
                        () =>
                            new ResponseModel<R>
                            {
                                IsSuccess = false,
                                FaultMessageOnFailure = new ResponseException
                                {
                                    ApiErrorMessage = exception.Message,
                                    ErrorCode = System.Net.HttpStatusCode.InternalServerError,
                                    StackTrace = exception.StackTrace
                                },
                                Data = default(R)
                            }
                    );
            }
            catch (System.Exception exception)
            {
                return System.Threading.Tasks.Task<ResponseModel<R>>
                    .Factory
                    .StartNew
                    (
                        () =>
                            new ResponseModel<R>
                            {
                                IsSuccess = false,
                                FaultMessageOnFailure = new ResponseException
                                {
                                    ApiErrorMessage = exception.Message,
                                    ErrorCode = System.Net.HttpStatusCode.InternalServerError,
                                    StackTrace = exception.StackTrace
                                },
                                Data = default(R)
                            }
                    );
            }
        }

        public System.Threading.Tasks.Task<ResponseModel<R>> Put<T, R>(string routes, dynamic payLoad)
        {
            try
            {
                return ServerResponseAsync<R>(ApiClient.PostAsync
                    (
                        routes,
                        new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payLoad),
                        System.Text.Encoding.UTF8, "application/json")
                    ).Result);
            }
            catch (System.Net.Http.HttpRequestException exception)
            {
                return System.Threading.Tasks.Task<ResponseModel<R>>
                    .Factory
                    .StartNew
                    (
                        () =>
                            new ResponseModel<R>
                            {
                                IsSuccess = false,
                                FaultMessageOnFailure = new ResponseException
                                {
                                    ApiErrorMessage = exception.Message,
                                    ErrorCode = System.Net.HttpStatusCode.InternalServerError,
                                    StackTrace = exception.StackTrace
                                },
                                Data = default(R)
                            }
                    );
            }
            catch (System.Exception exception)
            {
                return System.Threading.Tasks.Task<ResponseModel<R>>
                    .Factory
                    .StartNew
                    (
                        () =>
                            new ResponseModel<R>
                            {
                                IsSuccess = false,
                                FaultMessageOnFailure = new ResponseException
                                {
                                    ApiErrorMessage = exception.Message,
                                    ErrorCode = System.Net.HttpStatusCode.InternalServerError,
                                    StackTrace = exception.StackTrace
                                },
                                Data = default(R)
                            }
                    );
            }
        }

        private async System.Threading.Tasks.Task<ResponseModel<T>> ServerResponseAsync<T>(System.Net.Http.HttpResponseMessage result)
        {
            string data = string.Empty;

            switch (result.StatusCode)
            {
                // 1xx Series
                case System.Net.HttpStatusCode.Continue:
                case System.Net.HttpStatusCode.SwitchingProtocols:
                    data = await result.Content.ReadAsStringAsync();
                    return new ResponseModel<T>
                    {
                        IsSuccess = result.IsSuccessStatusCode,
                        FaultMessageOnFailure = null,
                        Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data)
                    };

                // 2xx Series
                case System.Net.HttpStatusCode.OK:
                case System.Net.HttpStatusCode.Created:
                case System.Net.HttpStatusCode.Accepted:
                case System.Net.HttpStatusCode.NonAuthoritativeInformation:
                case System.Net.HttpStatusCode.NoContent:
                case System.Net.HttpStatusCode.ResetContent:
                case System.Net.HttpStatusCode.PartialContent:
                    data = await result.Content.ReadAsStringAsync();
                    return new ResponseModel<T>
                    {
                        IsSuccess = result.IsSuccessStatusCode,
                        FaultMessageOnFailure = null,
                        Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data)
                    };


                // 3xx Series
                case System.Net.HttpStatusCode.MultipleChoices:
                case System.Net.HttpStatusCode.MovedPermanently:
                case System.Net.HttpStatusCode.Found:
                case System.Net.HttpStatusCode.SeeOther:
                case System.Net.HttpStatusCode.NotModified:
                case System.Net.HttpStatusCode.TemporaryRedirect:
                    data = await result.Content.ReadAsStringAsync();
                    return new ResponseModel<T>
                    {
                        IsSuccess = result.IsSuccessStatusCode,
                        FaultMessageOnFailure = null,
                        Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data)
                    };

                // 4xx Series
                case System.Net.HttpStatusCode.BadRequest:
                case System.Net.HttpStatusCode.Unauthorized:
                case System.Net.HttpStatusCode.PaymentRequired:
                case System.Net.HttpStatusCode.Forbidden:
                case System.Net.HttpStatusCode.NotFound:
                case System.Net.HttpStatusCode.MethodNotAllowed:
                case System.Net.HttpStatusCode.NotAcceptable:
                case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                case System.Net.HttpStatusCode.RequestTimeout:
                case System.Net.HttpStatusCode.Conflict:
                case System.Net.HttpStatusCode.Gone:
                case System.Net.HttpStatusCode.LengthRequired:
                case System.Net.HttpStatusCode.PreconditionFailed:
                case System.Net.HttpStatusCode.RequestEntityTooLarge:
                case System.Net.HttpStatusCode.RequestUriTooLong:
                case System.Net.HttpStatusCode.UnsupportedMediaType:
                case System.Net.HttpStatusCode.RequestedRangeNotSatisfiable:
                case System.Net.HttpStatusCode.ExpectationFailed:
                case System.Net.HttpStatusCode.UpgradeRequired:
                    return new ResponseModel<T>
                    {
                        IsSuccess = result.IsSuccessStatusCode,
                        FaultMessageOnFailure = new ResponseException
                        {
                            ApiErrorMessage = " - API Error - " + result.StatusCode.ToString(),
                            ErrorCode = result.StatusCode,
                            StackTrace = " - API Error - " + System.Environment.NewLine +
                                         result.RequestMessage.RequestUri.AbsoluteUri + System.Environment.NewLine +
                                         result.RequestMessage.Method.Method + System.Environment.NewLine +
                                         result.ReasonPhrase + System.Environment.NewLine +
                                         result.StatusCode
                        },
                        Data = default(T)
                    };

                // 5xx Series
                case System.Net.HttpStatusCode.InternalServerError:
                case System.Net.HttpStatusCode.NotImplemented:
                case System.Net.HttpStatusCode.BadGateway:
                case System.Net.HttpStatusCode.ServiceUnavailable:
                case System.Net.HttpStatusCode.GatewayTimeout:
                case System.Net.HttpStatusCode.HttpVersionNotSupported:
                    data = await result.Content.ReadAsStringAsync();
                    return new ResponseModel<T>
                    {
                        IsSuccess = result.IsSuccessStatusCode,
                        FaultMessageOnFailure = new ResponseException
                        {
                            ApiErrorMessage = System.Environment.NewLine + " - API Error - " + result.StatusCode.ToString(),
                            ErrorCode = result.StatusCode,
                            StackTrace = " - API Error - " + System.Environment.NewLine +
                                         result.RequestMessage.RequestUri.AbsoluteUri + System.Environment.NewLine +
                                         result.RequestMessage.Method.Method + System.Environment.NewLine +
                                         result.ReasonPhrase + System.Environment.NewLine +
                                         result.StatusCode
                        },
                        Data = default(T)
                    };

                default:
                    return new ResponseModel<T>
                    {
                        IsSuccess = false,
                        FaultMessageOnFailure = null,
                        Data = default(T)
                    };
            }
        }
    }

    public class ResponseModel<T>
    {
        public bool IsSuccess { get; set; }

        public ResponseException FaultMessageOnFailure { get; set; }

        public T Data { get; set; }

        public ResponseModel()
        {
            FaultMessageOnFailure = new ResponseException();
        }
    }

    public class ResponseException
    {
        public System.Net.HttpStatusCode ErrorCode { get; set; }

        public string ApiErrorMessage { get; set; }

        public string StackTrace { get; set; }
    }

    internal static class Extensions
    {
        public static void AddLogixRequestHeaders(this System.Net.Http.HttpClient client)
        {
            // TODO!
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
