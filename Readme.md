# seedtable

Rails等で扱うseed yaml <-> xlsx を相互変換するツールです。

簡単なGUIもあります。

## Motivation

xlsxは個人用単体表計算ソフトとしては優秀ですが、複数人でのデータ入力やプログラムでの扱い、バージョン管理などには超不向きです。
にもかかわらず、これをRailsアプリケーション等ののデータ入力ツールとして使うことがしばしば選択肢に上がるようです。
Excelをマスターデータとしたら最後、その不向きな部分の辛さが待っています。

それに対してこのseedtableは、yamlとxlsxの相互変換をすることで、Excelを利用しつつyamlをマスターデータとする運用の道を開きます。
マスターデータはyamlで、Excelはあくまで編集のしやすい表計算UIであるとして運用すれば、複数人管理、プログラム、バージョン管理親和性を損ねないまま、ユーザーフレンドリーなデータ入力を実現することができるのではないかと考えます。

Excelは大きな長所と同時に大きな短所をもち、運用上の扱い方が難しいツールです。
このツールが、そのようなExcelの長所だけを引き出すより良いデータ運用の一助になれば幸いです。

## Requires

.NET Framework 4.0だか4.6だかあたりが必要だと思います。 (Windows)

Monoだと4.4で動きます。(Mac / Linux)

## Install

[Releases](https://github.com/seed-ui/seedtable/releases)にあるzipをおとしてきてコマンドラインで叩いて下さい。

Monoの場合は`mono seedtable.exe`等として下さい。

## Usage (seedtable.exe)

```
# xlsx -> yaml
$ seedtable from foo.xlsx -o db/seeds
$ seedtable from foo.xlsx -o db/seeds -O hoges piyos
$ seedtable from foo.xlsx -o db/seeds -S hoges:2
$ seedtable from foo.xlsx -o db/seeds -S 1:hoges
$ seedtable from -i doc foo.xlsx -o db/seeds

# yaml -> xlsx
$ seedtable to -s db/seeds -x doc foo.xlsx -o newdoc
```

- 数式セルはxlsx -> yaml変換ではちゃんと計算後の値となります。yaml -> xlsx変換では上書きされず、数式のままとなります。
- xlsx -> yamlの変換のみならほぼ互換のオプションを持つ[xlsx2seed](https://github.com/Narazaka/xlsx2seed.js)もあります。

## Usage (seedtable-gui.exe)

- seedフォルダ (yaml -> xlsxの変換元、およびxlsx -> yamlの変換先になる)
- 設定ファイル (seedtable.exeのオプション相当)

を入力してから、「yml -> xlsx」、「xlsx -> yml」それぞれをダブルクリックするか、xlsxファイルをドラッグ&ドロップして変換できます。

設定ファイルは下記のように、seedtable.exeのオプションのうちinput/output系を除いた長いオプション名をyamlで設定します。

```yaml
ignore-columns:
  - dummy
subdivide:
  - "foos:0"
  - "bars:2"
engine: EPPlus
```

## Engines

seedtableではxlsxファイルを扱うバックエンドとして複数のライブラリを選択できます。

`-e`オプションに指定してください。

| ライブラリ | from (xlsx -> yaml) | to (yaml -> xlsx) |
|---|---|---|
| OpenXml | 速い | 行挿入削除× |
| EPPlus | 速い | 行挿入削除〇 |
| ClosedXML | 遅い 一部数式を含むxlsxはエラー | 行挿入削除× スタイルが保存されない |

OpenXmlとEPPlusのfrom処理速度はほぼ同じですが、ファイルによって3割程度の速度差がある場合があります。
ClosedXMLはその2倍以上程度の時間がかかります。

## オプションの注意点

- to処理においてsubdivide指定は考慮されません。指定にかかわらず自動判定して読み取ります。
- `-d, --delete`オプションはEPPlusエンジンでのみ動作します。

## 未実装機能

- `-R, --require-version`オプションは動作しません。

## Excelファイルの形式

以下のような形式を想定しています。

### Excel

シート名`characters`

|   | A  |  B   |      C      |
|---|----|------|-------------|
| 1 | ID | 名前 |    説明     |
| 2 | id | name | description |
| 3 | 1 | さくら | F.I.R.S.T |
| 4 | 2 | アルル | ボク |
| 5 | 3 | さっちゃんさん | 髪が長い |

### YAML

`characters.yml`

```
data1:
  id: 1
  name: さくら
  description: F.I.R.S.T
data2:
  id: 2
  name: アルル
  description: ボク
data3:
  id: 3
  name: さっちゃんさん
  description: 髪が長い
```

デフォルトではExcel表の2行目がカラム名、3行目以降がデータとして扱われますが、これらはそれぞれ`--column-names-row`、`--data-start-row`オプションで変更可能です。

## Contribute

バグ報告や要望等は[Issues](https://github.com/seed-ui/seedtable/issues)にお願いいたします。

[プルリク](https://github.com/seed-ui/seedtable/pulls)もざっくりお待ちしています。

ただし仕事の合間対応なので反応速度ははやいとは限りません。

## License

このソフトウェアにはApache Licenseのもとで頒布されているソフトウェアOpen-XML-SDKと、LGPLのもとで頒布されているソフトウェアEPPlusが使われています。

このソフトウェアは[MIT License](https://narazaka.net/license/MIT?2016)のもとでリリースされています。
