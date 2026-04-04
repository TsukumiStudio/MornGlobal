# MornGlobal

<p align="center">
  <img src="src/Editor/MornGlobal.png" alt="MornGlobal" width="640" />
</p>

<p align="center">
  <img src="https://img.shields.io/github/license/TsukumiStudio/MornGlobal" alt="License" />
</p>

## 概要

Mornモジュール共通のシングルトン管理・ログ・Editorユーティリティを提供する基盤ライブラリ。

## 導入方法

Unity Package Manager で以下の Git URL を追加:

```
https://github.com/TsukumiStudio/MornGlobal.git?path=src#1.0.0
```

`Window > Package Manager > + > Add package from git URL...` に貼り付けてください。

## 機能

3種類のシングルトンベースクラスを提供:

| クラス | 継承元 | 用途 |
|--------|--------|------|
| `MornGlobalBase<T>` | ScriptableObject | 設定アセット |
| `MornGlobalMonoBase<T>` | MonoBehaviour | ランタイムサービス |
| `MornGlobalPureBase<T>` | なし | Unity非依存のPure C#シングルトン |

## 使い方

### ScriptableObject シングルトン

```csharp
[CreateAssetMenu(menuName = "Morn/MyGlobal")]
public sealed class MyGlobal : MornGlobalBase<MyGlobal>
{
    protected override string ModuleName => "MyModule";
    protected override Color ModuleColor => Color.cyan; // 省略時は green
    [SerializeField] private float _speed = 1f;
    public float Speed => _speed;
}
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

### 共通アクセス（全Base共通）

```csharp
// シングルトンへのアクセス
var speed = MyGlobal.I.Speed;

// ログ出力
MyGlobal.Logger.Log("初期化完了");
MyGlobal.Logger.LogWarning("値が未設定です");
MyGlobal.Logger.LogError("エラー発生", this);

// Editor SetDirty（ランタイムで呼んでも安全）
MornGlobalUtil.SetDirty(target);
```

## ライセンス

[The Unlicense](LICENSE)
