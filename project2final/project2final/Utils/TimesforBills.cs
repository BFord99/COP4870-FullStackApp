using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project2final.Models;


namespace project2final.Utils
{
    public record TimesforBills(ObservableCollection<Time> _timesForBills);
}
