using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace EPIC.MedicalControls.Shaders
{
    // Token: 0x02000062 RID: 98
    public class InvertColorEffect : ShaderEffect
    {
        // Token: 0x0600030E RID: 782 RVA: 0x000199A4 File Offset: 0x00017BA4
        static InvertColorEffect()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            InvertColorEffect.pixelShader = new PixelShader();
            string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
            foreach (string text in manifestResourceNames)
            {
                if (text.Contains("InvertColor.ps"))
                {
                    Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(text);
                    InvertColorEffect.pixelShader.SetStreamSource(manifestResourceStream);
                    break;
                }
            }
        }

        // Token: 0x0600030F RID: 783 RVA: 0x00019A31 File Offset: 0x00017C31
        public InvertColorEffect()
        {
            base.PixelShader = InvertColorEffect.pixelShader;
            base.UpdateShaderValue(InvertColorEffect.InputProperty);
        }

        // Token: 0x17000093 RID: 147
        // (get) Token: 0x06000310 RID: 784 RVA: 0x00019A54 File Offset: 0x00017C54
        // (set) Token: 0x06000311 RID: 785 RVA: 0x00019A76 File Offset: 0x00017C76
        [Browsable(false)]
        public Brush Input
        {
            get
            {
                return (Brush)base.GetValue(InvertColorEffect.InputProperty);
            }
            set
            {
                base.SetValue(InvertColorEffect.InputProperty, value);
            }
        }

        // Token: 0x04000174 RID: 372
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(InvertColorEffect), 0);

        // Token: 0x04000175 RID: 373
        private static readonly PixelShader pixelShader;
    }
}
