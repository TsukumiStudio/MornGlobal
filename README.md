# MornGlobal

## 概要

グローバル設定とシングルトン管理のシステムベースライブラリ。

## 依存関係

| 種別 | 名前 |
|------|------|
| Mornライブラリ | MornLib |

## 使い方

### カスタムグローバル設定の作成

```csharp
[CreateAssetMenu(menuName = "Morn/MyGlobal")]
public class MyGlobal : MornGlobalBase<MyGlobal>
{
    public string setting1;
    public int setting2;
}
```

### グローバル設定へのアクセス

```csharp
// シングルトンインスタンスにアクセス
var setting = MyGlobal.I.setting1;
```

### MonoBehaviour版

```csharp
public class MyGlobalMono : MornGlobalMonoBase<MyGlobalMono>
{
    // シーン上に配置して使用
}
```
