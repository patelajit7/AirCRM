namespace Infrastructure.HelpingModel
{
    public class Response<T>
    {
       public TransactionStatus TransactionStatus { get; set; }
       public T Result { get; set; }
    }

   public class TransactionStatus
   {
       public bool IsSuccess { get; set; }
       public Error Error { get; set; }

   }
   public class Error
   {
       public string Code { get; set; }
       public string Description { get; set; }

   }
}
