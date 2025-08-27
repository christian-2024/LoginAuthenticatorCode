namespace LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;

    public class PaginationHeaderDto<T>
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IReadOnlyList<T> Items { get; set; }

        public PaginationHeaderDto(int currentPage, int itemsPerPage, int totalItems, int totalPages, IReadOnlyList<T> items)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
            Items = items;
    }
}

