using Client.classDTO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using static Client.AutWindow;

namespace Client.pages
{
   
    public partial class PageCompetenceCoeff : Page
    {
        CompetenceCoeffsDTO[] coeffs;

        public float[,] matrixCoeff = new float[5,11];
        public float[,] matrix_buf = new float[5, 11];

        public PageCompetenceCoeff()
        {
            InitializeComponent();
            Loaded += PageCompetenceCoeff_Loaded;
        }

        private void PageCompetenceCoeff_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private async void LoadData() 
        {
            ConnectHost conn = new ConnectHost();
            JToken jObject = null;
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            jObject = await conn.GetCompetenceCoeffs();
            coeffs = JsonConvert.DeserializeObject<CompetenceCoeffsDTO[]>(jObject.ToString(), jsonSettings);
            foreach (var coefficient in coeffs) 
            {
                matrixCoeff[coefficient.IdCompetence, coefficient.IdGroup] = coefficient.Coefficient;
            }

            TB_1_1.Text = matrixCoeff[1, 1].ToString();
            TB_1_2.Text = matrixCoeff[1, 2].ToString();
            TB_1_3.Text = matrixCoeff[1, 3].ToString();
            TB_1_4.Text = matrixCoeff[1, 4].ToString();

            TB_2_1.Text = matrixCoeff[2, 1].ToString();
            TB_2_2.Text = matrixCoeff[2, 2].ToString();
            TB_2_3.Text = matrixCoeff[2, 3].ToString();
            TB_2_4.Text = matrixCoeff[2, 4].ToString();

            TB_3_1.Text = matrixCoeff[3, 1].ToString();
            TB_3_2.Text = matrixCoeff[3, 2].ToString();
            TB_3_3.Text = matrixCoeff[3, 3].ToString();
            TB_3_4.Text = matrixCoeff[3, 4].ToString();

            TB_4_1.Text = matrixCoeff[4, 1].ToString();
            TB_4_2.Text = matrixCoeff[4, 2].ToString();
            TB_4_3.Text = matrixCoeff[4, 3].ToString();
            TB_4_4.Text = matrixCoeff[4, 4].ToString();

            TB_1_5.Text = matrixCoeff[1, 5].ToString();
            TB_1_6.Text = matrixCoeff[1, 6].ToString();
            TB_1_7.Text = matrixCoeff[1, 7].ToString();
            TB_1_10.Text = matrixCoeff[1, 10].ToString();

            TB_2_5.Text = matrixCoeff[2, 5].ToString();
            TB_2_6.Text = matrixCoeff[2, 6].ToString();
            TB_2_7.Text = matrixCoeff[2, 7].ToString();
            TB_2_10.Text = matrixCoeff[2, 10].ToString();

            TB_3_5.Text = matrixCoeff[3, 5].ToString();
            TB_3_6.Text = matrixCoeff[3, 6].ToString();
            TB_3_7.Text = matrixCoeff[3, 7].ToString();
            TB_3_10.Text = matrixCoeff[3, 10].ToString();

            TB_4_5.Text = matrixCoeff[4, 5].ToString();
            TB_4_6.Text = matrixCoeff[4, 6].ToString();
            TB_4_7.Text = matrixCoeff[4, 7].ToString();
            TB_4_10.Text = matrixCoeff[4, 10].ToString();
        }

        private void sel_profile(object sender, SelectionChangedEventArgs e)
        {
            if ((MartixMex != null) && (MartixTex != null))
            {
                if (profileCB.SelectedIndex == 0)
                {
                    MartixMex.Visibility = Visibility.Visible;
                    MartixTex.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MartixMex.Visibility = Visibility.Collapsed;
                    MartixTex.Visibility = Visibility.Visible;
                }
            }
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private async void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            ConnectHost conn = new ConnectHost();

            matrix_buf = matrixCoeff;

            BT_UPD.IsEnabled = false;


            if (float.TryParse(TB_1_1.Text.Trim(), out float G1))
            {
                TB_1_1.Background = Brushes.White;
            }
            else
            {
                TB_1_1.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_2_1.Text.Trim(), out float G2))
            {
                TB_2_1.Background = Brushes.White;
            }
            else
            {
                TB_2_1.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_3_1.Text.Trim(), out float G3))
            {
                TB_3_1.Background = Brushes.White;
            }
            else
            {

                TB_3_1.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_4_1.Text.Trim(), out float G4))
            {
                TB_4_1.Background = Brushes.White;
            }
            else
            {
                TB_4_1.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            float sum1 = G1 + G2 + G3 + G4;

            if (sum1 != 1)
            {
                TB_1_1.Background = Brushes.Pink;
                TB_2_1.Background = Brushes.Pink;
                TB_3_1.Background = Brushes.Pink;
                TB_4_1.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнена Группа 1");
                BT_UPD.IsEnabled = true;
                return;
            }

            matrix_buf[1, 1] = G1;
            matrix_buf[2, 1] = G2;
            matrix_buf[3, 1] = G3;
            matrix_buf[4, 1] = G4;

            ///////////////////////////////////
            if (float.TryParse(TB_1_2.Text.Trim(), out G1))
            {
                TB_1_2.Background = Brushes.White;
            }
            else
            {
                TB_1_2.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_2_2.Text.Trim(), out G2))
            {
                TB_2_2.Background = Brushes.White;
            }
            else
            {
                TB_2_2.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_3_2.Text.Trim(), out G3))
            {
                TB_3_2.Background = Brushes.White;
            }
            else
            {

                TB_3_2.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_4_2.Text.Trim(), out G4))
            {
                TB_4_2.Background = Brushes.White;
            }
            else
            {
                TB_4_2.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            sum1 = G1 + G2 + G3 + G4;

            if (sum1 != 1)
            {
                TB_1_2.Background = Brushes.Pink;
                TB_2_2.Background = Brushes.Pink;
                TB_3_2.Background = Brushes.Pink;
                TB_4_2.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнена Группа 2");
                BT_UPD.IsEnabled = true;
                return;
            }

            matrix_buf[1, 2] = G1;
            matrix_buf[2, 2] = G2;
            matrix_buf[3, 2] = G3;
            matrix_buf[4, 2] = G4;

            ///////////////////////////////////
            if (float.TryParse(TB_1_3.Text.Trim(), out G1))
            {
                TB_1_3.Background = Brushes.White;
            }
            else
            {
                TB_1_3.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_2_3.Text.Trim(), out G2))
            {
                TB_2_3.Background = Brushes.White;
            }
            else
            {
                TB_2_3.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_3_3.Text.Trim(), out G3))
            {
                TB_3_3.Background = Brushes.White;
            }
            else
            {

                TB_3_3.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_4_3.Text.Trim(), out G4))
            {
                TB_4_3.Background = Brushes.White;
            }
            else
            {
                TB_4_3.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            sum1 = G1 + G2 + G3 + G4;

            if (sum1 != 1)
            {
                TB_1_3.Background = Brushes.Pink;
                TB_2_3.Background = Brushes.Pink;
                TB_3_3.Background = Brushes.Pink;
                TB_4_3.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнена Группа 3");
                BT_UPD.IsEnabled = true;
                return;
            }

            matrix_buf[1, 3] = G1;
            matrix_buf[2, 3] = G2;
            matrix_buf[3, 3] = G3;
            matrix_buf[4, 3] = G4;

            ///////////////////////////////////
            if (float.TryParse(TB_1_4.Text.Trim(), out G1))
            {
                TB_1_4.Background = Brushes.White;
            }
            else
            {
                TB_1_4.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_2_4.Text.Trim(), out G2))
            {
                TB_2_4.Background = Brushes.White;
            }
            else
            {
                TB_2_4.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_3_4.Text.Trim(), out G3))
            {
                TB_3_4.Background = Brushes.White;
            }
            else
            {

                TB_3_4.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_4_4.Text.Trim(), out G4))
            {
                TB_4_4.Background = Brushes.White;
            }
            else
            {
                TB_4_4.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            sum1 = G1 + G2 + G3 + G4;

            if (sum1 != 1)
            {
                TB_1_4.Background = Brushes.Pink;
                TB_2_4.Background = Brushes.Pink;
                TB_3_4.Background = Brushes.Pink;
                TB_4_4.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнена Группа 4");
                BT_UPD.IsEnabled = true;
                return;
            }

            matrix_buf[1, 4] = G1;
            matrix_buf[2, 4] = G2;
            matrix_buf[3, 4] = G3;
            matrix_buf[4, 4] = G4;

            //////////////////////////////////
            //////////////////////////////////

            if (float.TryParse(TB_1_5.Text.Trim(), out  G1))
            {
                TB_1_5.Background = Brushes.White;
            }
            else
            {
                TB_1_5.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_2_5.Text.Trim(), out  G2))
            {
                TB_2_5.Background = Brushes.White;
            }
            else
            {
                TB_2_5.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_3_5.Text.Trim(), out  G3))
            {
                TB_3_5.Background = Brushes.White;
            }
            else
            {

                TB_3_5.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_4_5.Text.Trim(), out  G4))
            {
                TB_4_5.Background = Brushes.White;
            }
            else
            {
                TB_4_5.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            sum1 = G1 + G2 + G3 + G4;

            if (sum1 != 1)
            {
                TB_1_5.Background = Brushes.Pink;
                TB_2_5.Background = Brushes.Pink;
                TB_3_5.Background = Brushes.Pink;
                TB_4_5.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнена Группа 1");
                BT_UPD.IsEnabled = true;
                return;
            }

            matrix_buf[1, 5] = G1;
            matrix_buf[2, 5] = G2;
            matrix_buf[3, 5] = G3;
            matrix_buf[4, 5] = G4;

            ///////////////////////////////////
            if (float.TryParse(TB_1_6.Text.Trim(), out G1))
            {
                TB_1_6.Background = Brushes.White;
            }
            else
            {
                TB_1_6.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_2_6.Text.Trim(), out G2))
            {
                TB_2_6.Background = Brushes.White;
            }
            else
            {
                TB_2_6.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_3_6.Text.Trim(), out G3))
            {
                TB_3_6.Background = Brushes.White;
            }
            else
            {

                TB_3_6.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_4_6.Text.Trim(), out G4))
            {
                TB_4_6.Background = Brushes.White;
            }
            else
            {
                TB_4_6.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            sum1 = G1 + G2 + G3 + G4;

            if (sum1 != 1)
            {
                TB_1_6.Background = Brushes.Pink;
                TB_2_6.Background = Brushes.Pink;
                TB_3_6.Background = Brushes.Pink;
                TB_4_6.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнена Группа 2");
                BT_UPD.IsEnabled = true;
                return;
            }

            matrix_buf[1, 6] = G1;
            matrix_buf[2, 6] = G2;
            matrix_buf[3, 6] = G3;
            matrix_buf[4, 6] = G4;

            ///////////////////////////////////
            if (float.TryParse(TB_1_7.Text.Trim(), out G1))
            {
                TB_1_7.Background = Brushes.White;
            }
            else
            {
                TB_1_7.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_2_7.Text.Trim(), out G2))
            {
                TB_2_7.Background = Brushes.White;
            }
            else
            {
                TB_2_7.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_3_7.Text.Trim(), out G3))
            {
                TB_3_7.Background = Brushes.White;
            }
            else
            {

                TB_3_7.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_4_7.Text.Trim(), out G4))
            {
                TB_4_7.Background = Brushes.White;
            }
            else
            {
                TB_4_7.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            sum1 = G1 + G2 + G3 + G4;

            if (sum1 != 1)
            {
                TB_1_7.Background = Brushes.Pink;
                TB_2_7.Background = Brushes.Pink;
                TB_3_7.Background = Brushes.Pink;
                TB_4_7.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнена Группа 3");
                BT_UPD.IsEnabled = true;
                return;
            }

            matrix_buf[1, 7] = G1;
            matrix_buf[2, 7] = G2;
            matrix_buf[3, 7] = G3;
            matrix_buf[4, 7] = G4;

            ///////////////////////////////////
            if (float.TryParse(TB_1_10.Text.Trim(), out G1))
            {
                TB_1_10.Background = Brushes.White;
            }
            else
            {
                TB_1_10.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_2_10.Text.Trim(), out G2))
            {
                TB_2_10.Background = Brushes.White;
            }
            else
            {
                TB_2_10.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_3_10.Text.Trim(), out G3))
            {
                TB_3_10.Background = Brushes.White;
            }
            else
            {

                TB_3_10.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            if (float.TryParse(TB_4_10.Text.Trim(), out G4))
            {
                TB_4_10.Background = Brushes.White;
            }
            else
            {
                TB_4_10.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнено");
                BT_UPD.IsEnabled = true;
                return;
            }

            sum1 = G1 + G2 + G3 + G4;

            if (sum1 != 1)
            {
                TB_1_10.Background = Brushes.Pink;
                TB_2_10.Background = Brushes.Pink;
                TB_3_10.Background = Brushes.Pink;
                TB_4_10.Background = Brushes.Pink;
                MessageBox.Show("Неверно заполнена Группа 4");
                BT_UPD.IsEnabled = true;
                return;
            }

            matrix_buf[1, 10] = G1;
            matrix_buf[2, 10] = G2;
            matrix_buf[3, 10] = G3;
            matrix_buf[4, 10] = G4;

            string jout = string.Empty;
            JToken jObject = null;
            bool ok = false;

            foreach (var tmpCoff in coeffs) 
            {
                if (tmpCoff.Coefficient != matrix_buf[tmpCoff.IdCompetence, tmpCoff.IdGroup])
                {
                    tmpCoff.Coefficient = matrix_buf[tmpCoff.IdCompetence, tmpCoff.IdGroup];
                    jout = JsonConvert.SerializeObject(tmpCoff);
                    jObject = await conn.UpdateCompetenceCoeff(jout);
                    if (jObject == null)
                    {
                        MessageBox.Show("Не удалось обновить значения коэффициентов");
                        BT_UPD.IsEnabled = true;
                        return;
                    }
                    ok = true;
                }
                

            }

            if (ok)
            {
                MessageBox.Show("Значения коэффициентов обновлены");
                matrixCoeff = matrix_buf;
            }

            BT_UPD.IsEnabled = true;

        }
    }
}
