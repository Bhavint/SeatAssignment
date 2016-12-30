using System.Collections.Generic;

namespace SeatAssignment.Entities
{
    public class ValidationResult
    {
        public ValidationStatus Status;

        public List<string> Messages;

        public ValidationResult()
        {
            Status = ValidationStatus.Failure;
            Messages = new List<string>();
        }
    }
}
