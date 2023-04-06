using FlightManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.ViewModels
{
    public class UserViewModel
    {
        public int currentPageIndex { get; set; }
        public int pageCount { get; set; }
        public IEnumerable<User> userList;

    }
}
