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

    public static class StatusHelper
    {
        public static string ToString(this StatusEnum status)
        {
            switch (status)
            {
                case StatusEnum.OPEN:
                    return "OPEN";
                case StatusEnum.PROCESSING:
                    return "PROCESSING";
                case StatusEnum.FINAL:
                    return"FINAL";
                case StatusEnum.CANCELED:
                    return "CANCELED";
                default:
                    return "UNDEFINED";
            }
        }
    }

}
