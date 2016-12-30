using MoreLinq;
using SeatAssignment.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SeatAssignment.BusinessLogic
{
    public static class InputValidator
    {
        public static ValidationResult Validate(List<ReservationRequest> requests)
        {
            var result = new ValidationResult();

            if (ContainsDuplicates(requests))
            {
                result.Status = ValidationStatus.Failure;
                result.Messages.Add(ValidationMessages.DuplicateValues);
            }
            if (ContainsExcessiveRequests(requests))
            {
                result.Status = ValidationStatus.Failure;
                result.Messages.Add(ValidationMessages.ExcessiveRequest);
            }

            if (!result.Messages.Any())
            {
                result.Status = ValidationStatus.Success;
            }

            return result;
        }

        private static bool ContainsDuplicates(List<ReservationRequest> requests)
        {
            var uniqueRequests = requests.DistinctBy(request => request.RequestId);
            return !(uniqueRequests.Count() == requests.Count());
        }

        private static bool ContainsExcessiveRequests(List<ReservationRequest> requests)
        {
            var totalSeatsRequested = requests.Sum(request => request.NumberOfSeats);
            return (totalSeatsRequested > ConfigurationReader.NumberOfRows * ConfigurationReader.SeatsInEachRow);
        }
    }
}
