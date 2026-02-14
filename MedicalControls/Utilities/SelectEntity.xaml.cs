using EPIC.MedicalControls.Extensions;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EPIC.MedicalControls.Controls
{
    // Token: 0x02000015 RID: 21
    public partial class SelectEntity : Window
    {
        // Token: 0x06000135 RID: 309 RVA: 0x0000B0DD File Offset: 0x000092DD
        private SelectEntity()
        {
            this.InitializeComponent();
        }

        // Token: 0x06000136 RID: 310 RVA: 0x0000B108 File Offset: 0x00009308
        public static TEntity Show<TEntity>(IEnumerable<TEntity> source, IList<Expression<Func<TEntity, object>>> columns, IList<string> headers = null, string title = null)
        {
            SelectEntity selectEntity = new SelectEntity
            {
                Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault((Window x) => x.IsActive)
            };
            selectEntity.Entities.SelectionMode = DataGridSelectionMode.Single;
            if (!string.IsNullOrEmpty(title))
            {
                selectEntity.Title = title;
            }
            SelectEntity.SetupColumns<TEntity>(selectEntity, columns, headers);
            selectEntity.Entities.ItemsSource = source;
            return (selectEntity.ShowDialog() == true) ? ((TEntity)((object)selectEntity.Entities.SelectedItem)) : default(TEntity);
        }

        // Token: 0x06000137 RID: 311 RVA: 0x0000B1D4 File Offset: 0x000093D4
        public static IEnumerable<TEntity> ShowMulti<TEntity>(IEnumerable<TEntity> source, IList<Expression<Func<TEntity, object>>> columns, IList<string> headers = null, string title = null)
        {
            SelectEntity selectEntity = new SelectEntity
            {
                Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault((Window x) => x.IsActive)
            };
            selectEntity.Entities.SelectionMode = DataGridSelectionMode.Extended;
            if (!string.IsNullOrEmpty(title))
            {
                selectEntity.Title = title;
            }
            SelectEntity.SetupColumns<TEntity>(selectEntity, columns, headers);
            selectEntity.Entities.ItemsSource = source;
            return (selectEntity.ShowDialog() == true) ? selectEntity.Entities.SelectedItems.OfType<TEntity>() : new List<TEntity>();
        }

        // Token: 0x06000138 RID: 312 RVA: 0x0000B280 File Offset: 0x00009480
        private static void SetupColumns<TEntity>(SelectEntity select, IList<Expression<Func<TEntity, object>>> columns, IList<string> headers)
        {
            foreach (Expression<Func<TEntity, object>> expression in columns)
            {
                string expressionText = expression.GetExpressionText();
                if (typeof(FrameworkElement).IsAssignableFrom(expression.Body.Type))
                {
                    FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(ContentPresenter));
                    frameworkElementFactory.SetBinding(ContentPresenter.ContentProperty, new Binding(expressionText));
                    select.Entities.Columns.Add(new DataGridTemplateColumn
                    {
                        CellTemplate = new DataTemplate
                        {
                            VisualTree = frameworkElementFactory
                        },
                        Header = ((headers != null) ? headers[columns.IndexOf(expression)] : expression.Name)
                    });
                }
                else
                {
                    select.Entities.Columns.Add(new DataGridTextColumn
                    {
                        Binding = new Binding(expressionText),
                        Header = ((headers != null) ? headers[columns.IndexOf(expression)] : expression.Name)
                    });
                }
            }
            select.Entities.Columns.Add(new DataGridTextColumn
            {
                Width = new DataGridLength(100.0, DataGridLengthUnitType.Star),
                CanUserSort = false
            });
        }

        // Token: 0x06000139 RID: 313 RVA: 0x0000B410 File Offset: 0x00009610
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            base.DialogResult = new bool?(true);
            base.Close();
        }

        // Token: 0x0600013A RID: 314 RVA: 0x0000B428 File Offset: 0x00009628
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            base.DialogResult = null;
            base.Close();
        }
    }
}
