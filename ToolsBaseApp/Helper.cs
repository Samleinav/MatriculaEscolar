using ModelsBaseApp;
using System;
using System.Globalization;

namespace ToolsBaseApp
{
    public class Helper
    {
        
        public static MethodResponse IsTimeMatricula(string inicia, string fin)
        {
            if (!IsDate(inicia) || !IsDate(fin)) return new MethodResponse() { MSG = ResError.FECHA_NO_CALCULABLE };
        

        var IniciaDate = Convert.ToDateTime(inicia);
            var FinDate = Convert.ToDateTime(fin);
            var now = DateTime.Now;
            // see if start comes before end
            if (IniciaDate < FinDate)
                if (IniciaDate <= now && now <= FinDate)
                {
                    return new MethodResponse()
                    {
                        result = true,
                        type = "success",
                        MSG = ResMSG.System.PUEDE_MATRICULAR

                    };
                }
            // start is after end, so do the inverse comparison
            if (!(now > FinDate || now < IniciaDate))
            {
                return new MethodResponse()
                {
                    result = true,
                    type = "warning",
                    MSG = ResError.FUERA_DE_RANGO_FECHA

                };
            }
            return new MethodResponse()
            {
                result = true,
                type = "error",
                MSG = ResError.FECHA_NO_CALCULABLE

            };
        }
        public static bool IsDate(string readAddMeeting)
        {

            var dateFormats = new[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };

            DateTime scheduleDate;
            var validDate = DateTime.TryParseExact(
               readAddMeeting,
               dateFormats,
               DateTimeFormatInfo.InvariantInfo,
               DateTimeStyles.None,
               out scheduleDate);
            return validDate;
        }
    }

}
