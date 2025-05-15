namespace GestionStockMedIHM.Domain.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public static ApiResponse<T> SuccessResponse(T data, string message = "Opération réussie")
        {
            return new() {
                Success = true,
                Data = data,
                Message = message 
            };
        }

        public static ApiResponse<T> ErrorResponse(string errorMessage, List<string> errors = null)
        {
            return new()
               {
                   Success = false,
                   Message = errorMessage,
                   Errors = errors ?? new List<string>()
               };
        }
    }
}

