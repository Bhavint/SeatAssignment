using SeatAssignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeatAssignment.Entities;
using Microsoft.Practices.Unity;

namespace SeatAssignment.BusinessLogic
{
    /// <summary>
    /// Sorts reservation requests to assign contiguous seats to larger groups on priority
    /// </summary>
    public class LargeGroupFavoringTheaterManager : ITheaterManager
    {
        [Dependency("internalTheaterManager")]
        protected ITheaterManager _internalTheaterManager { get; set; }

        public List<ReservationAssignment> AssignSeats(List<ReservationRequest> requests)
        {
            var sortedRequests = requests.OrderByDescending(request => request.NumberOfSeats).ToList();
            return _internalTheaterManager.AssignSeats(sortedRequests);
        }
    }
}
