# PYEditor_Develop
PYEditor開発用プロジェクト  
  
Unityで使えるエディタ拡張やシェーダーの詰め合わせ  
Unity 2019.4.31f1で制作・動作確認

整えられてないプロジェクトなのでまだ未完成だったりバグが残ってたりするのはご愛嬌  
なにかあればissuesにお願いします  
  
# How to use
リポジトリをクローンするか、右上・緑色の”Code”をクリックして”Download ZIP"します。  
  
クローンするか、ダウンロード後解凍したフォルダを開くと  
PYEditor_Develop/Assets/  
にエディタ拡張とシェーダーがあります。必要なものをお使いのプロジェクトにD&Dでインポートしてください。  
  
インポートに成功すると画面上のツールバーに"PYEditor"と項目が追加されます。  
  
※Githubのページから必要なものだけ直接DLしても大丈夫

# 完成品  
- InactiveCollidersearcher  
└シーン内のInactiveなコライダーをコンソールに列挙するやーつ
- GlobalRotationResetter  
└オブジェクトsのRotationをグローバル軸でどうこうするやーつ
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

一部は説明書がついてたりついてなかったりします。  
また、拡張内で特筆無い限り基本Undoが実装されています。
  
# Licence
PYEditor  
Copyright (c) 2022 RyutoThurusaki  
  
This software is released under the MIT license.  
https://opensource.org/licenses/MIT

Assets以外のフォルダに入っているアセット/コードに関してはそれぞれのライセンスに準拠します。

