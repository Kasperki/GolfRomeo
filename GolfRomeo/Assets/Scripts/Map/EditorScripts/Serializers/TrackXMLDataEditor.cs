using System.Xml;

public class TrackXMLDataEditor
{
    private string trackName;
    private TrackFolderHelper trackFolderHelper;

    public TrackXMLDataEditor(string trackName)
    {
        this.trackName = trackName;
        trackFolderHelper = new TrackFolderHelper();
    }

    private string TrackXMLFilePath
    {
        get
        {
            return trackFolderHelper.GetTrackPath(trackName) + TrackSerializer.mapFileExtension;
        }
    }

    private XmlDocument GetTrackXMLFile()
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(TrackXMLFilePath);

        return xml;
    }

    public void ChangeTrackName(string newTrackName)
    {
        var xml = GetTrackXMLFile();
        xml.SelectSingleNode("/" + Constants.Track_Root_Node_Name + "/MapName").InnerText = newTrackName;
        xml.Save(TrackXMLFilePath);
    }

    public void ChangeTrackRecord(float lapTime)
    {
        var xml = GetTrackXMLFile();
        xml.SelectSingleNode("/" + Constants.Track_Root_Node_Name + "/TrackRecord").InnerText = lapTime.ToString();
        xml.Save(TrackXMLFilePath);
    }

    public float GetTrackRecord()
    {
        var xml = GetTrackXMLFile();

        float trackRecord = 0;
        float.TryParse(xml.SelectSingleNode("/" + Constants.Track_Root_Node_Name + "/TrackRecord").InnerText, out trackRecord);

        return trackRecord;
    }
}
