﻿#if UNITY_EDITOR

using System.Text;
using UnityEngine;
using System.IO;

namespace Utage
{
    //シナリオファイル読み込み後の処理として、ログファイルを出力する
    [CreateAssetMenu(menuName = "Utage/CustomPostProcessor/ScenarioLogFile", fileName = "ScenarioLogFile")]
    public class ScenarioFileReadPostprocessorLogFile : ScenarioFileReadPostprocessor
    {
        public Encoding Encoding => string.IsNullOrEmpty(encoding) ? Encoding.UTF8 : Encoding.GetEncoding(encoding);
        [SerializeField] string encoding = "";

        [SerializeField] string logDirectoryName = "ScenarioFileLog~";

        //シナリオファイル読み込み後の処理
        public override void OnScenarioFileReadPostprocess(AdvScenarioImporterInEditor importerInEditor, string path,
            StringGridDictionary gridDictionary)
        {
            //ユーザー設定で無効になっていたらなにもしない
            var editorUserSettings = UtageEditorUserSettings.GetInstance().ImportSetting;
            if(!editorUserSettings.EnableScenarioLogFile ) return;

            CsvParser parser = new CsvParser(){Encoding = this.Encoding};
            foreach (var stringGridDictionaryKeyValue in gridDictionary.List)
            {
                var grid = stringGridDictionaryKeyValue.Grid;
                //元のファイルパス以下にログ用のフォルダを作成し、ログファイルを出力する
                string dir = Path.GetDirectoryName(path);
                dir = Path.Combine(dir ?? string.Empty, logDirectoryName);
                dir = Path.Combine(dir, Path.GetFileNameWithoutExtension(path));
                string outputPath = Path.Combine(dir, grid.SheetName+".csv");
                //ログファイルの出力
                parser.WriteFile(outputPath, stringGridDictionaryKeyValue.Grid);                
            }
        }
    }
}

#endif
