# MornGlobal

<p align="center">
  <img src="Editor/MornGlobal.png" alt="MornGlobal" width="640" />
</p>

<p align="center">
  <img src="https://img.shields.io/github/license/TsukumiStudio/MornGlobal" alt="License" />
</p>

## 概要

Mornモジュール共通のシングルトン管理・ログ・Editorユーティリティを提供する基盤ライブラリ。

## 導入方法

Unity Package Manager で以下の Git URL を追加:

```
https://github.com/TsukumiStudio/MornGlobal.git
```

`Window > Package Manager > + > Add package from git URL...` に貼り付けてください。

## 機能

### シングルトン基盤

3種類のシングルトンベースクラスを提供:

| クラス                     | 継承元              | 用途                            |
|-------------------------|------------------|-------------------------------|
| `MornGlobalBase<T>`     | ScriptableObject | 設定アセット。PreloadedAssetsに自動登録   |
| `MornGlobalMonoBase<T>` | MonoBehaviour    | ランタイムサービス。DontDestroyOnLoad対応 |
| `MornGlobalPureBase<T>` | なし               | Unity非依存のPure C#シングルトン        |

- **自動アセット作成** — `MornGlobalBase` は初回アクセス時にアセット作成ダイアログを表示し、PreloadedAssetsに自動登録
- **重複防止** — `MornGlobalMonoBase` はAwake時に重複インスタンスを自動破棄

### モジュールログ

- **カラープレフィックス** — `[ModuleName]` 付きでログを出力
- **レベル別制御** — EditorPrefsでモジュール単位にLog / Warning / Error の表示をON/OFF
- **リリースビルド抑制** — `Debug.isDebugBuild` が false の場合はログを出力しない

### Editorユーティリティ

- **`MornGlobalUtil.SetDirty()`** — `EditorUtility.SetDirty` のランタイム安全ラッパー
- **`MornGlobalPreloader`** — PreloadedAssetsのHideFlagsを自動修正

## 使い方

### ScriptableObject シングルトン

```csharp
[CreateAssetMenu(menuName = "Morn/MyGlobal")]
public sealed class MyGlobal : MornGlobalBase<MyGlobal>
{
    protected override string ModuleName => "MyModule";
    [SerializeField] private float _speed = 1f;
    public float Speed => _speed;
}
```

```csharp
// 設定値へのアクセス
var speed = MyGlobal.I.Speed;

// ログ出力
MyGlobal.Logger.Log("初期化完了");
MyGlobal.Logger.LogWarning("値が未設定です");

// Editor SetDirty
MornGlobalUtil.SetDirty(target);
```

### MonoBehaviour シングルトン

```csharp
public sealed class MyService : MornGlobalMonoBase<MyService>
{
    protected override string ModuleName => "MyService";
    protected override void OnInitialized() { /* 初期化処理 */ }
}
```

### Pure C# シングルトン

```csharp
public sealed class MyPure : MornGlobalPureBase<MyPure>
{
    protected override string ModuleName => "MyPure";
}
```

## ライセンス

[The Unlicense](LICENSE)
