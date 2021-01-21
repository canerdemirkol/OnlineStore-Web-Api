namespace OnlineStore.Core.Common.Contracts.RequestMessages
{
    public class CategoryUpdateRequest:CategoryCreateRequest
    {
        public int Id { get; set; }
    }
}