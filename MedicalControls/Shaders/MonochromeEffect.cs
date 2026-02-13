using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace EPIC.MedicalControls.Shaders
{
    // Token: 0x02000063 RID: 99
    public class MonochromeEffect : ShaderEffect
    {
        // Token: 0x06000312 RID: 786 RVA: 0x00019A88 File Offset: 0x00017C88
        static MonochromeEffect()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            MonochromeEffect.pixelShader = new PixelShader();
            string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
            foreach (string text in manifestResourceNames)
            {
                if (text.Contains("Monochrome.ps"))
                {
                    Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(text);
                    MonochromeEffect.pixelShader.SetStreamSource(manifestResourceStream);
                    break;
                }
            }
        }

        // Token: 0x06000313 RID: 787 RVA: 0x00019B4D File Offset: 0x00017D4D
        public MonochromeEffect()
        {
            base.PixelShader = MonochromeEffect.pixelShader;
            base.UpdateShaderValue(MonochromeEffect.FilterColorProperty);
            base.UpdateShaderValue(MonochromeEffect.InputProperty);
        }

        // Token: 0x17000094 RID: 148
        // (get) Token: 0x06000314 RID: 788 RVA: 0x00019B7C File Offset: 0x00017D7C
        // (set) Token: 0x06000315 RID: 789 RVA: 0x00019B9E File Offset: 0x00017D9E
        public Color FilterColor
        {
            get
            {
                return (Color)base.GetValue(MonochromeEffect.FilterColorProperty);
            }
            set
            {
                base.SetValue(MonochromeEffect.FilterColorProperty, value);
            }
        }

        // Token: 0x17000095 RID: 149
        // (get) Token: 0x06000316 RID: 790 RVA: 0x00019BB4 File Offset: 0x00017DB4
        // (set) Token: 0x06000317 RID: 791 RVA: 0x00019BD6 File Offset: 0x00017DD6
        [Browsable(false)]
        public Brush Input
        {
            get
            {
                return (Brush)base.GetValue(MonochromeEffect.InputProperty);
            }
            set
            {
                base.SetValue(MonochromeEffect.InputProperty, value);
            }
        }

        // Token: 0x04000176 RID: 374
        public static readonly DependencyProperty FilterColorProperty = DependencyProperty.Register("FilterColor", typeof(Color), typeof(MonochromeEffect), new UIPropertyMetadata(Colors.White, ShaderEffect.PixelShaderConstantCallback(0)));

        // Token: 0x04000177 RID: 375
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(MonochromeEffect), 0);

        // Token: 0x04000178 RID: 376
        private static PixelShader pixelShader;
    }
}
