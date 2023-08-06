using project2final.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project2final.Utils
{
    public record sendClients_bills(ObservableCollection<Bill> _bills);
}
