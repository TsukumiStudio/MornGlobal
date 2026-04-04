# MornGlobal

Singleton & Logging Core

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

| クラス | 継承元 | 用途 |
|--------|--------|------|
| `MornGlobalBase<T>` | ScriptableObject | 設定アセット。PreloadedAssetsに自動登録 |
| `MornGlobalMonoBase<T>` | MonoBehaviour | ランタイムサービス。DontDestroyOnLoad対応 |
| `MornGlobalPureBase<T>` | なし | Unity非依存のPure C#シングルトン |

- **自動アセット作成** — `MornGlobalBase` は初回アクセス時にアセット作成ダイアログを表示し、PreloadedAssetsに自動登録
- **重複検知** — `MornGlobalBase` は重複アセットをポップアップで通知し自動削除。`MornGlobalMonoBase` は重複GameObjectをLogErrorで通知し自動破棄

### モジュールログ

- **カラープレフィックス** — `[ModuleName]` 付きでログを出力。色は `IMornGlobal.ModuleColor` で変更可能（デフォルト: green）
- **リリースビルド除外** — `[Conditional]` 属性により、Editor・DevelopmentBuild以外ではログ呼び出し自体がコンパイルから除外
- **Debug.Log互換** — `(object message)` と `(object message, Object context)` の両オーバーロードに対応

### Editorユーティリティ

- **`MornGlobalUtil.SetDirty()`** — `EditorUtility.SetDirty` のランタイム安全ラッパー
- **`MornGlobalUtil.EnsurePreloadedAsset()`** — ScriptableObjectをPreloadedAssetsに確実に登録
- **`MornGlobalUtil.FindOrCreatePreloadedAsset()`** — PreloadedAssetsから検索、なければ作成ダイアログを表示
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
MyGlobal.Logger.LogError("エラー発生", this);

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

### ログ色のカスタマイズ

```csharp
public sealed class MyGlobal : MornGlobalBase<MyGlobal>
{
    protected override string ModuleName => "MyModule";
    Color IMornGlobal.ModuleColor => Color.cyan;
}
```

## ライセンス

[The Unlicense](LICENSE)
