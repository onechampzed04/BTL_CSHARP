using BTL_2.Model;
using BTL_2.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_2.Controller
{
    public class AddressController
    {
        private Dictionary<string, Province> _provinces;
        public ComboBox cbxProvince { get; private set; }
        public ComboBox cbxWard {  get; private set; }
        public ComboBox cbxDistrict { get; private set; }

        public AddressController(ComboBox cbxProvince, ComboBox cbxDistrict,ComboBox cbxWard)
        {
            this.cbxProvince = cbxProvince;
            this.cbxWard = cbxWard;
            this.cbxDistrict = cbxDistrict;
            LoadData();
            PopulateProvinces();
            SetEvent();
        }

        private void LoadData()
        {
            // Đường dẫn tương đối tới file JSON trong thư mục Resources
            var relativePath = @"C:\Users\Ash\Source\Repos\BTL_CSHARP1\Resources\dist.json";
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file was not found.", filePath);
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                _provinces = JsonConvert.DeserializeObject<Dictionary<string, Province>>(json);
            }
        }

        private void PopulateProvinces()
        {
            cbxProvince.Items.Clear();
            foreach (var province in _provinces.Values)
            {
                cbxProvince.Items.Add(province);
            }
        }

        public void SetEvent()
        {
            cbxProvince.SelectedIndexChanged += new System.EventHandler(comboBoxProvince_SelectedIndexChanged);
            cbxDistrict.SelectedIndexChanged += new System.EventHandler(comboBoxDistrict_SelectedIndexChanged);
        }

        private void comboBoxProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedProvince = cbxProvince.SelectedItem as Province;
            if (selectedProvince != null)
            {
                cbxDistrict.Items.Clear();
                cbxWard.Items.Clear();
                foreach (var district in selectedProvince.Districts.Values)
                {
                    cbxDistrict.Items.Add(district);
                }
            }
        }

        private void comboBoxDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedDistrict = cbxDistrict.SelectedItem as District;
            if (selectedDistrict != null)
            {
                cbxWard.Items.Clear();
                foreach (var ward in selectedDistrict.Wards.Values)
                {
                    cbxWard.Items.Add(ward);
                }
            }
        }

        public void SetComboBoxSelection(string provinceNameWithType, string districtNameWithType, string wardNameWithType)
        {
            // Chọn tỉnh theo provinceNameWithType
            foreach (var province in cbxProvince.Items)
            {
                if (province is Province && ((Province)province).NameWithType == provinceNameWithType)
                {
                    cbxProvince.SelectedItem = province;
                    break;
                }
            }

            // Chọn huyện theo districtNameWithType
            var selectedProvince = cbxProvince.SelectedItem as Province;
            if (selectedProvince != null)
            {
                foreach (var district in cbxDistrict.Items)
                {
                    if (district is District && ((District)district).NameWithType == districtNameWithType)
                    {
                        cbxDistrict.SelectedItem = district;
                        break;
                    }
                }
            }

            // Chọn xã theo wardNameWithType
            var selectedDistrict = cbxDistrict.SelectedItem as District;
            if (selectedDistrict != null)
            {
                foreach (var ward in cbxWard.Items)
                {
                    if (ward is Ward && ((Ward)ward).NameWithType == wardNameWithType)
                    {
                        cbxWard.SelectedItem = ward;
                        break;
                    }
                }
            }
        }

    }
}
