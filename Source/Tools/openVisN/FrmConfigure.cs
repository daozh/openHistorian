﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GSF.TimeSeries;

namespace openVisN
{
    public partial class FrmConfigure : Form
    {
        SettingsManagement m_settings;
        public FrmConfigure(SettingsManagement settings)
        {
            m_settings = settings;
            InitializeComponent();
            dgvMeasurements.DataSource = settings.MyData.Tables["Measurements"];
            dgvTerminals.DataSource = settings.MyData.Tables["Terminals"];
            TxtGEPPort.Text = settings.GEPPort;
            TxtHistorianInstance.Text = settings.HistorianDatabase;
            TxtHistorianPort.Text = settings.HistorianPort;
            TxtServerIP.Text = settings.ServerIP;
        }

        private void BtnGetMetadata_Click(object sender, EventArgs e)
        {
            // Do the following on button click or missing configuration, etc:

            // Note that openHistorian internal publisher controls how many tables / fields to send as meta-data to subscribers (user controllable),
            // as a result, not all fields in associated database views will be available. Below are the default SELECT filters the publisher
            // will apply to the "MeasurementDetail", "DeviceDetail" and "PhasorDetail" database views:

            // SELECT NodeID, UniqueID, OriginalSource, IsConcentrator, Acronym, Name, ParentAcronym, ProtocolName, FramesPerSecond, Enabled FROM DeviceDetail WHERE IsConcentrator = 0
            // SELECT Internal, DeviceAcronym, DeviceName, SignalAcronym, ID, SignalID, PointTag, SignalReference, Description, Enabled FROM MeasurementDetail
            // SELECT DeviceAcronym, Label, Type, Phase, SourceIndex FROM PhasorDetail

            DataTable measurementTable = null;
            DataTable deviceTable = null;
            DataTable phasorTable = null;

            string server = "Server=" + TxtServerIP.Text.Trim() + "; Port=" + TxtGEPPort.Text.Trim() + "; Interface=0.0.0.0";
            try
            {
                DataSet metadata = MetadataRetriever.GetMetadata(server);

                // Reference meta-data tables
                measurementTable = metadata.Tables["MeasurementDetail"];
                deviceTable = metadata.Tables["DeviceDetail"];
                phasorTable = metadata.Tables["PhasorDetail"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception retrieving meta-data: " + ex.Message);
            }

            if ((object)measurementTable != null)
            {
                // Could filter measurements if desired (e.g., no stats)
                DataRow[] measurements = measurementTable.Select("SignalAcronym <> 'STAT' and SignalAcronym <> 'DIGI'");

                m_settings.MyData.Tables["Measurements"].Rows.Clear();

                // Do something with measurement records
                foreach (DataRow measurement in measurements)
                {
                    Guid signalID;
                    MeasurementKey measurementKey;
                    string historianInstance;
                    uint pointID;
                    string signalType;

                    //           table.Columns.Add("PointID", typeof(int));
                    //table.Columns.Add("SignalID", typeof(Guid));
                    //table.Columns.Add("Description", typeof(string));
                    //table.Columns.Add("DeviceName", typeof(string));

                    Guid.TryParse(measurement["SignalID"].ToString(), out signalID);
                    MeasurementKey.TryParse(measurement["ID"].ToString(), signalID, out measurementKey);

                    historianInstance = measurementKey.Source;
                    pointID = measurementKey.ID;

                    signalType = measurement["SignalAcronym"].ToString();

                    m_settings.MyData.Tables["Measurements"].Rows.Add((int)pointID, signalID, measurement["DeviceName"], signalType, measurement["Description"]);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            m_settings.GEPPort = TxtGEPPort.Text;
            m_settings.HistorianDatabase = TxtHistorianInstance.Text;
            m_settings.HistorianPort = TxtHistorianPort.Text;
            m_settings.ServerIP = TxtServerIP.Text;
            m_settings.Save();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void BtnRefreshGroups_Click(object sender, EventArgs e)
        {
            SortedSet<string> uniqueNames = new SortedSet<string>();
            DataTable measurements = m_settings.MyData.Tables["Measurements"];
            foreach (DataRow row in measurements.Rows)
            {
                uniqueNames.Add((string)row["DeviceName"]);
            }

            m_settings.MyData.Tables["Terminals"].Rows.Clear();


            foreach (string term in uniqueNames)
            {
                List<object> items = new List<object>();
                items.Add(term);
                items.Add(null); //Nominal Voltage
                AddIfExists(items, term, "IPHM", measurements);
                AddIfExists(items, term, "IPHA", measurements);
                AddIfExists(items, term, "VPHM", measurements);
                AddIfExists(items, term, "VPHA", measurements);
                AddIfExists(items, term, "DFDT", measurements);
                AddIfExists(items, term, "FREQ", measurements);
                AddIfExists(items, term, "FLAG", measurements);
                m_settings.MyData.Tables["Terminals"].Rows.Add(items.ToArray());

            }


        }

        void AddIfExists(List<object> items, string term, string type, DataTable measurements)
        {
            DataRow[] rows = measurements.Select(string.Format("DeviceName='{0}' and SignalAcronym='{1}'",term,type));
            if (rows.Length == 1)
                items.Add(rows[0]["PointID"]);
            else if (rows.Length>1)
                items.Add(DBNull.Value);
            else
                items.Add(DBNull.Value);
        }



    }
}