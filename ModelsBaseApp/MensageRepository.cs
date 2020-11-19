using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsBaseApp
{
    public static class ResMSG
    {

        static ResMSG()
        {
            License = new LicenseMSG();
            Config = new ConfigMSG();
            System = new SystemMSG();
        }

        public static LicenseMSG License { get; set; }
        public static ConfigMSG Config { get; set; }
        public static SystemMSG System { get; set; }


    }

    public static class ResError
    {
        static ResError()
        {
            License = new LicenseMSG();
        }
        public static string[] FUERA_DE_RANGO_FECHA { get { return MensageBuild.Return("FUERA_DE_RANGO_FECHA", "prueba de texto"); } }
        public static string[] FECHA_NO_CALCULABLE { get { return MensageBuild.Return("FECHA_NO_CALCULABLE", "prueba de texto"); } }
        public static string[] NO_EXISTE_ESTUDIANTE { get { return MensageBuild.Return("NO_EXISTE_ESTUDIANTE", "prueba de texto"); } }
        public static string[] NO_SE_HA_PODIDO_ACTUALIZAR { get { return MensageBuild.Return("NO_SE_HA_PODIDO_ACTUALIZAR", "prueba de texto"); } }
        public static string[] NO_SE_HA_PODIDO_ACTUALIZAR_DATOS { get { return MensageBuild.Return("LICENCIA_ACTIVADA", "prueba de texto"); } }
        public static string[] NO_SE_PUEDE_ACTIVAR { get { return MensageBuild.Return("NO_SE_HA_PODIDO_ACTUALIZAR_DATOS", "prueba de texto"); } }
        public static string[] PROBLEMA_CARGAR_ESTUDIANTES { get { return MensageBuild.Return("PROBLEMA_CARGAR_ESTUDIANTES", "prueba de texto"); } }
        public static string[] SIN_DATOS { get { return MensageBuild.Return("SIN_DATOS", "prueba de texto"); } }
        public static string[] NO_SE_PUDO_GUARDAR { get { return MensageBuild.Return("NO_SE_PUDO_GUARDAR", "prueba de texto"); } }
        public static string[] NO_EXISTE_CONFIGURACION { get { return MensageBuild.Return("NO_EXISTE_CONFIGURACION", "prueba de texto"); } }
        public static string[] DOCUMENTO_NO_GUARDADO { get { return MensageBuild.Return("DOCUMENTO_NO_GUARDADO", "prueba de texto"); } }
        public static string[] NO_SE_HA_PODIDO_ELIMINAR { get { return MensageBuild.Return("NO_SE_HA_PODIDO_ELIMINAR", "prueba de texto"); } }

        public static LicenseMSG License { get; set; }
    }

    public class LicenseMSG
    {
        public string[] LICENCIA_ACTIVADA { get { return MensageBuild.Return("LICENCIA_ACTIVADA", "prueba de texto"); } }
        public string[] LICENCIA_DESACTIVADA { get { return MensageBuild.Return("LICENCIA_DESACTIVADA", "prueba de texto"); } }
        public string[] LICENCIA_VALIDA { get { return MensageBuild.Return("LICENCIA_VALIDA", "prueba de texto"); } }
        public string[] LICENCIA_INVALIDA { get { return MensageBuild.Return("LICENCIA_INVALIDA", "prueba de texto"); } }
        public string[] LICENCIA_VENCIDA { get { return MensageBuild.Return("LICENCIA_VENCIDA", "prueba de texto"); } }
        public string[] LICENCIA_REVOCADA { get { return MensageBuild.Return("LICENCIA_REVOCADA", "prueba de texto"); } }
        public string[] LICENCIA_ERROR { get { return MensageBuild.Return("LICENCIA_ERROR", "prueba de texto"); } }
        public string[] LICENCIA_EN_USO { get { return MensageBuild.Return("LICENCIA_EN_USO", "prueba de texto"); } }
        public string[] LICENCIA_NO_ACTIVADA { get { return MensageBuild.Return("LICENCIA_NO_ACTIVADA", "prueba de texto"); } }

    }
  
    public class ConfigMSG
    {
        public string[] CONFIGURACION_GUARDADA { get { return MensageBuild.Return("CONFIGURACION_GUARDADA", "prueba de texto"); } }
        public string[] CONFIGURACION_NO_EXISTE { get { return MensageBuild.Return("CONFIGURACION_NO_EXISTE", "prueba de texto"); } }
        public string[] CONFIGURACION_ACTUALIZADO { get { return MensageBuild.Return("CONFIGURACION_ACTUALIZADO", "prueba de texto"); } }
        public string[] INFORMACION_GUARDADA { get { return MensageBuild.Return("INFORMACION_GUARDADA", "prueba de texto"); } }
        public string[] CONFIGURACION_GLOBAL_ERROR { get { return MensageBuild.Return("CONFIGURACION_GLOBAL_ERROR", "prueba de texto"); } }
    }
    public class SystemMSG
    {
        public string[] ERROR_SERVIDOR { get { return MensageBuild.Return("ERROR_SERVIDOR", "No se ha podido tener comunicación con el servidor principal, por favor vuelva a internar en unos minutos o comuníquese con Soporte Técnico."); } }
        public string[] PUEDE_MATRICULAR { get { return MensageBuild.Return("PUEDE_MATRICULAR", "prueba de texto"); } }
        public string[] PUEDE_IMPORTAR_ESTUDIANTE { get { return MensageBuild.Return("PUEDE_IMPORTAR_ESTUDIANTE", "prueba de texto"); } }
        public string[] EXISTE_ESTUDIANTE { get { return MensageBuild.Return("EXISTE_ESTUDIANTE", "prueba de texto"); } }
        public string[] DOCUMENTO_GUARDADO { get { return MensageBuild.Return("DOCUMENTO_GUARDADO", "prueba de texto"); } }
        public string[] CAMBIOS_GUARDADOS { get { return MensageBuild.Return("CAMBIOS_GUARDADOS", "prueba de texto"); } }
        public string[] NO_SE_HA_GUARDADO_CORRECTAMENTE { get { return MensageBuild.Return("NO_SE_HA_GUARDADO_CORRECTAMENTE", "No se ha podido detectar cambios en la configuración, por favor vuelva a realizarlos y asegúrese de guardar antes de salir."); } }
    }

    static class MensageBuild
    {
        public static string[] Return(string tittle, string mensage)
        {
            return new string[] { tittle, mensage };
        }
    }
}
