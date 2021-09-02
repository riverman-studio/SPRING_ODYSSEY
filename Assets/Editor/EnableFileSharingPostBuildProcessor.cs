using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class EnableFileSharingPostBuildProcessor
{
    [PostProcessBuildAttribute(1)]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        Debug.Log("EnableFileSharingPostBuildProcessor for target " + buildTarget + " at path " + pathToBuiltProject);
        if (buildTarget == BuildTarget.iOS)
        {
            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

            rootDict.SetBoolean("UIFileSharingEnabled", true);

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}


class EnableFileSharingPostBuildProcessor2 : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("EnableFileSharingPostBuildProcessor2 for target " + report.summary.platform + " at path " + report.summary.outputPath);
        if (report.summary.platform == BuildTarget.iOS)
        {
            
            // Get plist
            string plistPath = report.summary.outputPath + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

            rootDict.SetBoolean("UIFileSharingEnabled", true);

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}