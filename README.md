# PYEditor_Develop
PYEditor開発用プロジェクト  
  
Unityで使えるエディタ拡張やシェーダーの詰め合わせ  
Unity 2019.4.31f1で制作・動作確認

整えられてないプロジェクトなのでまだ未完成だったりバグが残ってたりするのはご愛嬌  
なにかあればissuesにお願いします  
  
# How to use
1. 以下のURLをコピーします。

```
https://github.com/RyutoThurusaki/PYEditor_Develop.git
```

2. 導入したいUnityProjectを開きます。  
3. Window/PackageManager を開きます。  
4. 右上の＋をクリック、"Add package from git url..."をクリック。  
5. 出てきたウィンドウに1でコピーしたURLをペーストしAdd。  
6. ツールバーにPYEditorが表示されたら正常にインストールできています。

   ![Installation](https://user-images.githubusercontent.com/41750783/210133266-9f022bde-3561-4fb1-857b-c60608bf6423.gif)


# 完成品  
### エディタ拡張
- InactiveCollidersearcher  
└シーン内のInactiveなコライダーをコンソールに列挙するやーつ
- GlobalRotationResetter  
└オブジェクトsのグローバルRotationを指定値に書き換えするやーつ
- MaterialRandamizer  
└オブジェクトsのマテリアルをn個のマテリアルからランダムに設定してくやーつ
- PrefabReplacer  
└配置済みのPrefab_AをPrefab_Bにまとめて置き換えるやーつ
- RotationRandamizer  
└オブジェクトsのRotationをそれぞれ秩序をもってランダムにするやーつ
- TransPadding  
└オブジェクトsの距離感そのままに間隔を広げたり狭めたりするやーつ
- SceneSelector  
└Assets/Scenes 直下のシーンファイルを一覧表示、ワンクリックで開いたりするやーつ
- 3DCursor
└Blenderの3Dカーソルを移植して、オブジェクトの移動を便利にしたやーつ
  
### シェーダー
- TextureMix  
└頂点カラーによってテクスチャと質感をミックスするやーつ　Albedo、Normal、Metaric、Smoothnessを各2つづつブレンドが可能
- AlbedoFusionShader  
└２つのテクスチャをAdditiveやMultiplyでミックスするやーつ　テクスチャ1はUV1、テクスチャ2はUV4を使用。テクスチャ2はUVスクロールも可能

一部は説明書がついてたりついてなかったりします。  
また、拡張内で特筆無い限り基本Undoが実装されています。
  
# Licence
PYEditor  
Copyright (c) 2022 RyutoThurusaki  
  
This software is released under the MIT license.  
https://opensource.org/licenses/MIT

一部のシェーダーや拡張はライセンスの例外があるので必要に応じてソースコード内の原文を確認してください。
