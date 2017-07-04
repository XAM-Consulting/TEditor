using System;
using TEditor.Abstractions;

namespace TEditor
{
    public class CrossTEditor
    {
        static Lazy<ITEditor> Implementation = new Lazy<ITEditor>(() => CreateTEditor(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static ITEditor Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static ITEditor CreateTEditor()
        {
#if PORTABLE
            return null;
#else
            return new TEditorImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }


        /// <summary>
        /// Dispose of everything 
        /// </summary>
        public static void Dispose()
        {
            if (Implementation != null && Implementation.IsValueCreated)
            {
                Implementation.Value.Dispose();

                Implementation = new Lazy<ITEditor>(() => CreateTEditor(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
            }
        }

        public static string PageTitle { get; set; } = "HTML Editor";
        public static string SaveText { get; set; } = "Save";
        public static string CancelText { get; set; } = "Cancel";
    }
}
