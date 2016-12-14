using FullInspector;

public class UpdateFullInspectorRootDirectory : fiSettingsProcessor {
    public void Process() {
        fiSettings.RootDirectory = "Assets/Scripts/FullInspector/FullInspector2/";
        fiSettings.RootGeneratedDirectory = "Assets/Scripts/FullInspector/FullInspector2_Generated/";
    }
}
