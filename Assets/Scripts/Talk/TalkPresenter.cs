using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
// using Utage;

public class TalkPresenter : MonoBehaviour
{
    private TalkModel talkModel;
    
    [SerializeField] private TalkView talkView;
    
    /*
    private AdvEngine advEngine;*/

    private void Start()
    {
        Bind();
    }

    private void Bind()
    {
        
    }
    
    public void ReceiveTalk(string message)
    {
        // 現在はログに表示（将来 Utage と差し替える）
        Debug.Log($"[TalkPresenter] AIの返答: {message}");
        talkView.SetText(message);
    }
    
       /*public AdvEngine engine;
       public RuntimeScenarioImporter scenarioImporter;

       public void MakeScenarioAndLoad(List<string> Scenario)
       {
           //シナリオ名。エクセルのシート名に相当
           const string scenarioName = "SampleRuntimeScenario";
           StringGrid scenario = MakeScenario(scenarioName , Scenario);
           //StringGridDictionary scenarioをインポートして、シナリオをロード
           if (!scenarioImporter.TryImportAndLoadScenario(scenario))
           {
               Debug.LogError("Failed Import Scenario " + scenarioName, this);
               return;
           }
       }*/

       //「宴」形式のシナリオを作成
       /*StringGrid MakeScenario(string scenarioName,List<String> scenario)
       {
           StringGrid grid = new StringGrid(scenarioName, scenarioName, CsvType.Csv);
           grid.AddRow(new List<string>
           {
               //ヘッダー部分
               "Command", "Arg1", "Arg2", "Arg3", "Arg4", "Arg5", "Arg6", "WaitType", "Text", "PageCtrl", "Voice",
               "WindowType"
           });
           grid.ParseHeader();
       
           var ConvertedScenario = AdvCommandParser.IdPauseScenario;
           //コマンド記述の例
           grid.AddRow(new List<string>
           {
               AdvCommandParser.IdPauseScenario, scenario[0], scenario[1], scenario[2], scenario[3], scenario[4], scenario[5], scenario[6], scenario[7],
               scenario[8], scenario[9], scenario[10]
           });
           return grid;
       }*/
}
