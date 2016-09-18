# seedtable

seed yaml <-> xlsx を相互変換するツール

## Usage

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

## 未実装機能

- `-R, --require-version`オプションは動作しません
- to処理においてsubdivideは考慮されません
- `-d, --delete`オプションはEPPlusエンジンでのみ動作します

## License

このソフトウェアにはApache Licenseのもとで頒布されているソフトウェアOpen-XML-SDKと、LGPLのもとで頒布されているソフトウェアEPPlusが使われています。

このソフトウェアは[MIT License](https://narazaka.net/license/MIT?2016)のもとでリリースされています。
