namespace Domain.Models.Carts
{
    public class Cart
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public void ValidateOwnership(Guid? currentUserId)
        {
            if (currentUserId is null && UserId.HasValue)
                throw new InvalidOperationException("This cart belongs to a registered user. Please login to continue.");

            if (currentUserId.HasValue && UserId.HasValue && UserId.Value != currentUserId.Value)
                throw new InvalidOperationException("You do not have permission to access this cart.");
        }

        public void AssignToUser(Guid? userId)
        {
            if (userId.HasValue && !UserId.HasValue)
                UserId = userId;
        }

        public void MarkUpdated() => UpdatedAt = DateTime.UtcNow;
    }
}
