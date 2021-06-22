using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Models
{
    public enum StatusEnum
    {
        OPEN,         //przyjęte, jeszcze nie robimy
        PROCESSING,   //teraz robimy, robota trwa
        FINAL,        //zakończone
        CANCELED      //zakończone niepowodzeniem, anulowane
    }
}
