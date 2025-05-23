﻿/*
Copyright (c) 2020 Omar Duarte
Unauthorized copying of this file, via any medium is strictly prohibited.
Writen by Omar Duarte, 2020.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PluginMaster
{
    #region BRUSH SETTINGS
    [System.Serializable]
    public class BrushSettings
    {
        [SerializeField] private float _surfaceDistance = 0f;

        [SerializeField] protected bool _embedInSurface = false;
        [SerializeField] protected bool _embedAtPivotHeight = true;
        [SerializeField] protected Vector3 _localPositionOffset = Vector3.zero;
        [SerializeField] private bool _rotateToTheSurface = true;
        [SerializeField] private Vector3 _eulerOffset = Vector3.zero;
        [SerializeField] private bool _addRandomRotation = false;
        [SerializeField] private float _rotationFactor = 90;
        [SerializeField] private bool _rotateInMultiples = false;
        [SerializeField]
        private RandomUtils.Range3 _randomEulerOffset = new RandomUtils.Range3(Vector3.zero, Vector3.zero);
        [SerializeField] private bool _separateScaleAxes = false;
        [SerializeField] private Vector3 _scaleMultiplier = Vector3.one;
        [SerializeField] private bool _randomScaleMultiplier = false;
        [SerializeField]
        private RandomUtils.Range3 _randomScaleMultiplierRange = new RandomUtils.Range3(Vector3.one, Vector3.one);

        [SerializeField] private ThumbnailSettings _thumbnailSettings = new ThumbnailSettings();

        public virtual float surfaceDistance
        {
            get => _surfaceDistance;
            set
            {
                if (_surfaceDistance == value) return;
                _surfaceDistance = value;
            }
        }
        public virtual bool embedInSurface
        {
            get => _embedInSurface;
            set
            {
                if (_embedInSurface == value) return;
                _embedInSurface = value;
            }
        }
        public virtual bool embedAtPivotHeight
        {
            get => _embedAtPivotHeight;
            set
            {
                if (_embedAtPivotHeight == value) return;
                _embedAtPivotHeight = value;
            }
        }
        public virtual void UpdateBottomVertices() { }
        public virtual Vector3 localPositionOffset
        {
            get => _localPositionOffset;
            set
            {
                if (_localPositionOffset == value) return;
                _localPositionOffset = value;
            }
        }
        public virtual bool rotateToTheSurface
        {
            get => _rotateToTheSurface;
            set
            {
                if (_rotateToTheSurface == value) return;
                _rotateToTheSurface = value;
            }
        }
        public virtual Vector3 eulerOffset
        {
            get => _eulerOffset;
            set
            {
                if (_eulerOffset == value) return;
                _eulerOffset = value;
                _randomEulerOffset.v1 = _randomEulerOffset.v2 = Vector3.zero;
            }
        }
        public virtual bool addRandomRotation
        {
            get => _addRandomRotation;
            set
            {
                if (_addRandomRotation == value) return;
                _addRandomRotation = value;
            }
        }
        public virtual float rotationFactor
        {
            get => _rotationFactor;
            set
            {
                value = Mathf.Max(value, 0f);
                if (_rotationFactor == value) return;
                _rotationFactor = value;
            }
        }
        public virtual bool rotateInMultiples
        {
            get => _rotateInMultiples;
            set
            {
                if (_rotateInMultiples == value) return;
                _rotateInMultiples = value;
            }
        }
        public virtual RandomUtils.Range3 randomEulerOffset
        {
            get => _randomEulerOffset;
            set
            {
                if (_randomEulerOffset == value) return;
                _randomEulerOffset = value;
                _eulerOffset = Vector3.zero;
            }
        }
        public virtual bool separateScaleAxes
        {
            get => _separateScaleAxes;
            set
            {
                if (_separateScaleAxes == value) return;
                _separateScaleAxes = value;
            }
        }
        public virtual Vector3 scaleMultiplier
        {
            get => _scaleMultiplier;
            set
            {
                if (_scaleMultiplier == value) return;
                _scaleMultiplier = value;
                _randomScaleMultiplierRange.v1 = _randomScaleMultiplierRange.v2 = Vector3.one;
            }
        }
        public virtual RandomUtils.Range3 randomScaleMultiplierRange
        {
            get => _randomScaleMultiplierRange;
            set
            {
                if (_randomScaleMultiplierRange == value) return;
                _randomScaleMultiplierRange = value;
                _scaleMultiplier = Vector3.one;
            }
        }
        public virtual bool randomScaleMultiplier
        {
            get => _randomScaleMultiplier;
            set
            {
                if (_randomScaleMultiplier == value) return;
                _randomScaleMultiplier = value;
                _randomScaleMultiplierRange.v1 = _randomScaleMultiplierRange.v2 = _scaleMultiplier = Vector3.one;
            }
        }

        public virtual ThumbnailSettings thumbnailSettings
        {
            get => _thumbnailSettings;
            set => _thumbnailSettings.Copy(value);
        }

        public virtual BrushSettings Clone()
        {
            var clone = new BrushSettings();
            clone.Copy(this);
            return clone;
        }

        public virtual void Copy(BrushSettings other)
        {
            _surfaceDistance = other._surfaceDistance;
            _embedInSurface = other._embedInSurface;
            _embedAtPivotHeight = other._embedAtPivotHeight;
            _localPositionOffset = other._localPositionOffset;
            _rotateToTheSurface = other._rotateToTheSurface;
            _addRandomRotation = other._addRandomRotation;
            _eulerOffset = other._eulerOffset;
            _randomEulerOffset = new RandomUtils.Range3(other._randomEulerOffset);
            _randomScaleMultiplier = other._randomScaleMultiplier;
            _separateScaleAxes = other._separateScaleAxes;
            _scaleMultiplier = other._scaleMultiplier;
            _randomScaleMultiplierRange = new RandomUtils.Range3(other._randomScaleMultiplierRange);
            _thumbnailSettings.Copy(other._thumbnailSettings);
            _rotationFactor = other._rotationFactor;
            _rotateInMultiples = other._rotateInMultiples;
        }
        public BrushSettings() { }
        public BrushSettings(BrushSettings other) => Copy(other);
    }

    public static class SelectionUtils
    {
        public static void Swap<T>(int fromIdx, int toIdx, ref int[] selection, System.Collections.Generic.List<T> list)
        {
            if (fromIdx == toIdx) return;
            var newOrder = new System.Collections.Generic.List<T>();
            var newSelection = selection.ToArray();
            for (int idx = 0; idx <= list.Count; ++idx)
            {
                if (idx == toIdx)
                {
                    System.Array.Sort(selection);
                    int newSelectionIdx = 0;
                    foreach (var selectionIdx in selection)
                    {
                        newOrder.Add(list[selectionIdx]);
                        newSelection[newSelectionIdx++] = newOrder.Count - 1;
                    }
                    if (idx < list.Count && !selection.Contains(idx)) newOrder.Add(list[idx]);
                }
                else if (selection.Contains(idx)) continue;
                else if (idx < list.Count) newOrder.Add(list[idx]);
            }
            selection = newSelection;
            list.Clear();
            list.AddRange(newOrder);
            PWBCore.staticData.Save();
        }
    }

    [System.Serializable]
    public class ThumbnailSettings
    {
        [SerializeField] private Color _backgroudColor = Color.gray;
        [SerializeField] private Vector2 _lightEuler = new Vector2(130, -165);
        [SerializeField] private Color _lightColor = Color.white;
        [SerializeField] private float _lightIntensity = 1;
        [SerializeField] private float _zoom = 1;
        [SerializeField] private Vector3 _targetEuler = new Vector3(0, 125, 0);
        [SerializeField] private Vector2 _targetOffset = Vector2.zero;

        public Color backgroudColor { get => _backgroudColor; set => _backgroudColor = value; }
        public Vector2 lightEuler { get => _lightEuler; set => _lightEuler = value; }
        public Color lightColor { get => _lightColor; set => _lightColor = value; }
        public float lightIntensity { get => _lightIntensity; set => _lightIntensity = value; }
        public float zoom { get => _zoom; set => _zoom = value; }
        public Vector3 targetEuler { get => _targetEuler; set => _targetEuler = value; }
        public Vector2 targetOffset { get => _targetOffset; set => _targetOffset = value; }
        public ThumbnailSettings() { }
        public ThumbnailSettings(Color backgroudColor, Vector3 lightEuler, Color lightColor, float lightIntensity,
            float zoom, Vector3 targetEuler, Vector2 targetOffset)
        {
            _backgroudColor = backgroudColor;
            _lightEuler = lightEuler;
            _lightColor = lightColor;
            _lightIntensity = lightIntensity;
            _zoom = zoom;
            _targetEuler = targetEuler;
            _targetOffset = targetOffset;
        }

        public ThumbnailSettings(ThumbnailSettings other) => Copy(other);
        public void Copy(ThumbnailSettings other)
        {
            _backgroudColor = other._backgroudColor;
            _lightEuler = other._lightEuler;
            _lightColor = other._lightColor;
            _lightIntensity = other._lightIntensity;
            _zoom = other._zoom;
            _targetEuler = other._targetEuler;
            _targetOffset = other._targetOffset;
        }

        public ThumbnailSettings Clone()
        {
            var clone = new ThumbnailSettings();
            clone.Copy(this);
            return clone;
        }
    }

    [System.Serializable]
    public class MultibrushItemSettings : BrushSettings, ISerializationCallbackReceiver
    {
        [SerializeField] private bool _overwriteSettings = false;
        [SerializeField] private string _guid = string.Empty;
        [SerializeField] private string _prefabPath = string.Empty;
        [SerializeField] private float _frequency = 1;
        [SerializeField] private long _parentId = -1;
        [SerializeField] private bool _overwriteThumbnailSettings = false;
        [SerializeField] private bool _includeInThumbnail = true;
        [SerializeField] private bool _is2DAsset = false;
        private Vector3[] _bottomVertices = null;
        private float _bottomMagnitude = 0;
        private float _height = 1f;
        private Vector3 _size = Vector3.zero;
        private GameObject _prefab = null;
        private Texture2D _thumbnail = null;

        [System.NonSerialized] private MultibrushSettings _parentSettings = null;
        public MultibrushSettings parentSettings
        {
            get
            {
                if (_parentSettings == null) _parentSettings = PaletteManager.GetBrushById(_parentId);
                return _parentSettings;
            }
        }

        private void SavePalette()
        {
            if (parentSettings == null) return;
            parentSettings.SavePalette();
        }
        public MultibrushItemSettings(GameObject prefab, MultibrushSettings parentSettings)
        {
            _prefab = prefab;
            _parentId = parentSettings.id;
            _parentSettings = parentSettings;
            UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(_prefab, out _guid, out long localId);
            if (_prefab == null) return;
            _prefabPath = UnityEditor.AssetDatabase.GetAssetPath(_prefab);
            _bottomVertices = BoundsUtils.GetBottomVertices(prefab.transform);
            _height = BoundsUtils.GetBoundsRecursive(prefab.transform, prefab.transform.rotation).size.y;
            _size = BoundsUtils.GetBoundsRecursive(prefab.transform).size;
            _bottomMagnitude = BoundsUtils.GetBottomMagnitude(prefab.transform);
            UpdateAssetType();
        }

        public void InitializeParentSettings(MultibrushSettings parentSettings)
        {
            _parentId = parentSettings.id;
            _parentSettings = parentSettings;
            this.parentSettings.UpdateTotalFrequency();
        }

        public Texture2D thumbnail
        {
            get
            {
                if (_thumbnail == null) UpdateThumbnail();
                return _thumbnail;
            }
        }

        public Texture2D thumbnailTexture
        {
            get
            {
                if (_thumbnail == null) _thumbnail = new Texture2D(ThumbnailUtils.SIZE, ThumbnailUtils.SIZE);
                return _thumbnail;
            }
            set => _thumbnail = value;
        }

        public void UpdateThumbnail() => ThumbnailUtils.UpdateThumbnail(this);

        public bool overwriteSettings
        {
            get => _overwriteSettings;
            set
            {
                if (_overwriteSettings == value) return;
                _overwriteSettings = value;
                SavePalette();
            }
        }

        public float frequency
        {
            get => _frequency;
            set
            {
                value = Mathf.Max(value, 0);
                if (_frequency == value) return;
                _frequency = value;
                if (parentSettings != null) parentSettings.UpdateTotalFrequency();
            }
        }
        public GameObject prefab
        {
            get
            {
                if (_prefab == null)
                    _prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>
                        (UnityEditor.AssetDatabase.GUIDToAssetPath(_guid));
                if (_prefab == null)
                {
                    _prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(_prefabPath);
                    if (_prefab != null)
                        UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(_prefab, out _guid, out long localId);
                }
                else _prefabPath = UnityEditor.AssetDatabase.GetAssetPath(_prefab);
                if (_prefab == null && _parentSettings != null)
                {
                    if (PaletteManager.selectedBrush != null && PaletteManager.selectedBrush.id == _parentSettings.id)
                        PaletteManager.ClearSelection();
                }
                return _prefab;
            }
        }

        public override float surfaceDistance
            => _overwriteSettings || parentSettings == null ? base.surfaceDistance : parentSettings.surfaceDistance;

        public override bool embedInSurface
        {
            get => _overwriteSettings || parentSettings == null ? base.embedInSurface
                : parentSettings.embedInSurface;
            set
            {
                if (_embedInSurface == value) return;
                _embedInSurface = value;
                if (_embedInSurface) UpdateBottomVertices();
            }
        }

        public override bool embedAtPivotHeight
        {
            get => _overwriteSettings || parentSettings == null ? base.embedAtPivotHeight : parentSettings.embedAtPivotHeight;
            set
            {
                if (_embedAtPivotHeight == value) return;
                _embedAtPivotHeight = value;
            }
        }

        public override Vector3 localPositionOffset
            => _overwriteSettings || parentSettings == null ? base.localPositionOffset : parentSettings.localPositionOffset;
        public override bool rotateToTheSurface
            => _overwriteSettings || parentSettings == null ? base.rotateToTheSurface : parentSettings.rotateToTheSurface;
        public override Vector3 eulerOffset
            => _overwriteSettings || parentSettings == null ? base.eulerOffset : parentSettings.eulerOffset;
        public override bool addRandomRotation
            => _overwriteSettings || parentSettings == null ? base.addRandomRotation : parentSettings.addRandomRotation;
        public override RandomUtils.Range3 randomEulerOffset
            => _overwriteSettings || parentSettings == null ? base.randomEulerOffset : parentSettings.randomEulerOffset;
        public override float rotationFactor
            => _overwriteSettings || parentSettings == null ? base.rotationFactor : parentSettings.rotationFactor;
        public override bool rotateInMultiples
            => _overwriteSettings || parentSettings == null ? base.rotateInMultiples : parentSettings.rotateInMultiples;
        public override bool separateScaleAxes
            => _overwriteSettings || parentSettings == null ? base.separateScaleAxes : parentSettings.separateScaleAxes;
        public override Vector3 scaleMultiplier
            => _overwriteSettings || parentSettings == null ? base.scaleMultiplier : parentSettings.scaleMultiplier;
        public override RandomUtils.Range3 randomScaleMultiplierRange
            => _overwriteSettings || parentSettings == null ? base.randomScaleMultiplierRange
            : parentSettings.randomScaleMultiplierRange;
        public override bool randomScaleMultiplier
            => _overwriteSettings || parentSettings == null ? base.randomScaleMultiplier
            : parentSettings.randomScaleMultiplier;
        public Vector3 maxScaleMultiplier
            => randomScaleMultiplier ? randomScaleMultiplierRange.max : scaleMultiplier;
        public Vector3 minScaleMultiplier
            => randomScaleMultiplier ? randomScaleMultiplierRange.min : scaleMultiplier;
        public virtual bool overwriteThumbnailSettings
        {
            get => _overwriteThumbnailSettings;
            set
            {
                if (_overwriteThumbnailSettings == value) return;
                _overwriteThumbnailSettings = value;
            }
        }
        public override ThumbnailSettings thumbnailSettings
        {
            get => _overwriteThumbnailSettings || parentSettings == null
                ? base.thumbnailSettings : parentSettings.thumbnailSettings;
            set => base.thumbnailSettings = value;
        }
        public bool includeInThumbnail
        {
            get => _includeInThumbnail;
            set
            {
                if (_includeInThumbnail == value) return;
                _includeInThumbnail = value;
            }
        }

        public bool is2DAsset => _is2DAsset;

        public void UpdateAssetType() => _is2DAsset = Utils2D.Is2DAsset(prefab);
        public override void Copy(BrushSettings other)
        {
            if (other is MultibrushItemSettings)
            {
                var otherItemSettings = other as MultibrushItemSettings;
                _overwriteSettings = otherItemSettings._overwriteSettings;
                _frequency = otherItemSettings._frequency;
                _overwriteThumbnailSettings = otherItemSettings._overwriteThumbnailSettings;
                _includeInThumbnail = otherItemSettings._includeInThumbnail;
                _is2DAsset = otherItemSettings._is2DAsset;
            }
            base.Copy(other);
        }

        public MultibrushItemSettings() { }
        public MultibrushItemSettings(MultibrushItemSettings other) => Copy(other);
        public override BrushSettings Clone()
        {
            var clone = new MultibrushItemSettings();
            clone._prefab = _prefab;
            clone._thumbnail = _thumbnail;
            clone._guid = _guid;
            clone._parentId = parentSettings.id;
            clone._parentSettings = parentSettings;
            clone._bottomVertices = bottomVertices == null ? null : bottomVertices.ToArray();
            clone._bottomMagnitude = bottomMagnitude;
            clone._height = height;
            clone.Copy(this);
            return clone;
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            _prefab = null;
            _thumbnail = null;
        }

        public Vector3[] bottomVertices
        {
            get
            {
                if (_bottomVertices == null) UpdateBottomVertices();
                return _bottomVertices;
            }
        }

        public float bottomMagnitude
        {
            get
            {
                if (_bottomMagnitude == 0) _bottomMagnitude = BoundsUtils.GetBottomMagnitude(prefab.transform);
                return _bottomMagnitude;
            }
        }

        public float height => _height;
        public Vector3 size
        {
            get
            {
                if (_size == Vector3.zero) _size = BoundsUtils.GetBoundsRecursive(prefab.transform).size;
                return _size;
            }
        }
        public override void UpdateBottomVertices()
        {
            if (prefab == null) return;
            _bottomVertices = BoundsUtils.GetBottomVertices(prefab.transform);
            _height = BoundsUtils.GetBoundsRecursive(prefab.transform, prefab.transform.rotation).size.y;
            _size = BoundsUtils.GetBoundsRecursive(prefab.transform).size;
            _bottomMagnitude = BoundsUtils.GetBottomMagnitude(prefab.transform);
        }
    }

    [System.Serializable]
    public class MultibrushSettings : BrushSettings, ISerializationCallbackReceiver
    {
        public enum FrecuencyMode { RANDOM, PATTERN }
        [SerializeField] private long _id = -1;
        [SerializeField] private string _name = null;
        [SerializeField]
        private System.Collections.Generic.List<MultibrushItemSettings> _items
            = new System.Collections.Generic.List<MultibrushItemSettings>();
        [SerializeField] private FrecuencyMode _frequencyMode = FrecuencyMode.RANDOM;
        [SerializeField] private string _pattern = "1...";
        [SerializeField] private bool _restartPatternForEachStroke = true;

        [field: System.NonSerialized] private Texture2D _thumbnail = null;
        [field: System.NonSerialized] private float _totalFrequency = -1;
        [field: System.NonSerialized] private PatternMachine _patternMachine = null;
        [field: System.NonSerialized] private PaletteData _palette = null;

        public long id => _id;

        public string name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
            }
        }

        public FrecuencyMode frequencyMode
        {
            get => _frequencyMode;
            set
            {
                if (_frequencyMode == value) return;
                _frequencyMode = value;
            }
        }

        public string pattern
        {
            get => _pattern;
            set
            {
                if (_pattern == value) return;
                _pattern = value;
            }
        }

        public PatternMachine patternMachine
        {
            get
            {
                return _patternMachine;
            }
            set => _patternMachine = value;
        }

        public bool restartPatternForEachStroke
        {
            get => _restartPatternForEachStroke;
            set
            {
                if (_restartPatternForEachStroke == value) return;
                _restartPatternForEachStroke = value;
            }
        }

        public PaletteData palette
        {
            get
            {
                if (_palette == null) _palette = PaletteManager.GetPalette(this);
                return _palette;
            }
            set => _palette = value;
        }

        public bool isAsset2D => _items.Exists(i => i.is2DAsset);

        public void SavePalette()
        {
            if (palette != null) palette.Save();
        }
        public MultibrushSettings(GameObject prefab)
        {
            _id = System.DateTime.Now.Ticks;
            _items.Add(new MultibrushItemSettings(prefab, this));
            _name = prefab.name;
            Copy(PaletteManager.selectedPalette.brushCreationSettings.defaultBrushSettings);
            thumbnailSettings.Copy(PaletteManager.selectedPalette.brushCreationSettings.defaultThumbnailSettings);
        }

        public Texture2D thumbnail
        {
            get
            {
                if (_thumbnail == null) UpdateThumbnail();
                return _thumbnail;
            }
        }

        public Texture2D thumbnailTexture
        {
            get
            {
                if (_thumbnail == null) _thumbnail = new Texture2D(ThumbnailUtils.SIZE, ThumbnailUtils.SIZE);
                return _thumbnail;
            }
        }

        public void UpdateThumbnail() => ThumbnailUtils.UpdateThumbnail(this);


        public void AddItem(MultibrushItemSettings item)
        {
            _items.Add(item);
            OnItemCountChange();
        }

        private void RemoveFromPalette()
        {
            if (palette != null) palette.RemoveBrush(this);
        }

        public void RemoveItemAt(int index)
        {
            _items.RemoveAt(index);
            OnItemCountChange();
            if (_items.Count == 0) RemoveFromPalette();
        }

        public void RemoveItem(MultibrushItemSettings item)
        {
            if (!_items.Contains(item)) return;
            _items.Remove(item);
            OnItemCountChange();
            if (_items.Count == 0) RemoveFromPalette();
        }

        public MultibrushItemSettings GetItemAt(int index)
        {
            if (index >= _items.Count) return null;
            return _items[index];
        }

        public void InsertItemAt(MultibrushItemSettings item, int index)
        {
            _items.Insert(index, item);
            OnItemCountChange();
        }

        private void OnItemCountChange()
        {
            UpdateThumbnail();
            UpdateTotalFrequency();
            UpdatePatternMachine();
            PWBCore.staticData.Save();
            BrushstrokeManager.UpdateBrushstroke();
            SavePalette();
        }

        public void Swap(int fromIdx, int toIdx, ref int[] selection)
            => SelectionUtils.Swap<MultibrushItemSettings>(fromIdx, toIdx, ref selection, _items);

        public MultibrushItemSettings[] items => _items.ToArray();

        public int itemCount => _items.Count;

        public int notNullItemCount => _items.Where(i => i.prefab != null).Count();
        public bool containMissingPrefabs
        {
            get
            {
                foreach (var item in _items)
                    if (item.prefab == null) return true;
                return false;
            }
        }
        public void UpdateTotalFrequency()
        {
            _totalFrequency = 0;
            foreach (var item in _items) _totalFrequency += item.frequency;
        }

        public float totalFrecuency
        {
            get
            {
                if (_totalFrequency == -1) UpdateTotalFrequency();
                return _totalFrequency;
            }
        }
        public int nextItemIndex
        {
            get
            {
                if (frequencyMode == FrecuencyMode.RANDOM)
                {
                    if (_items.Count == 1) return 0;
                    var rand = UnityEngine.Random.Range(0f, totalFrecuency);
                    float sum = 0;
                    for (int i = 0; i < _items.Count; ++i)
                    {
                        sum += _items[i].frequency;
                        if (rand <= sum) return i;
                    }
                    return -1;
                }
                if (_patternMachine == null)
                {
                    if (PatternMachine.Validate(_pattern, _items.Count, out PatternMachine.Token[] tokens)
                        == PatternMachine.ValidationResult.VALID) _patternMachine = new PatternMachine(tokens);
                }
                return _patternMachine == null ? -2 : _patternMachine.nextIndex - 1;
            }
        }

        private void UpdatePatternMachine()
        {
            if (PatternMachine.Validate(_pattern, _items.Count, out PatternMachine.Token[] tokens)
                != PatternMachine.ValidationResult.VALID)
                _patternMachine = null;
        }

        public override void Copy(BrushSettings other)
        {

            if (other is MultibrushSettings)
            {
                var otherMulti = other as MultibrushSettings;
                _items.Clear();
                foreach (var item in otherMulti._items) _items.Add(item.Clone() as MultibrushItemSettings);
                _name = otherMulti._name;
                _frequencyMode = otherMulti._frequencyMode;
                _pattern = otherMulti._pattern;
                _restartPatternForEachStroke = otherMulti._restartPatternForEachStroke;
                _totalFrequency = otherMulti._totalFrequency;
            }
            base.Copy(other);
        }

        private MultibrushSettings()
        {
            _id = System.DateTime.Now.Ticks;
        }
        public override BrushSettings Clone()
        {
            var clone = new MultibrushSettings();
            clone.Copy(this);
            return clone;
        }

        public BrushSettings CloneMainSettings()
        {
            var clone = new BrushSettings();
            clone.Copy(this);
            return clone;
        }

        public void Duplicate(int index)
        {
            var clone = _items[index].Clone();
            _items.Insert(index, clone as MultibrushItemSettings);
            OnItemCountChange();
        }

        public void DuplicateAt(int indexToDuplicate, int at)
        {
            var clone = _items[indexToDuplicate].Clone();
            _items.Insert(at, clone as MultibrushItemSettings);
            OnItemCountChange();
        }

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() => _thumbnail = null;

        public override void UpdateBottomVertices()
        {
            foreach (var item in _items) item.UpdateBottomVertices();
        }

        public override bool embedInSurface
        {
            get => _embedInSurface;
            set
            {
                if (_embedInSurface == value) return;
                _embedInSurface = value;
                if (_embedInSurface) UpdateBottomVertices();
            }
        }
        public bool ContainsPrefab(int prefabId) => _items.Exists(item => item.prefab.GetInstanceID() == prefabId);
        public bool ContainsSceneObject(GameObject obj)
        {
            if (obj == null) return false;
            var outermostPrefab = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(obj);
            if (outermostPrefab == null) return false;
            var prefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(outermostPrefab);
            if (prefab == null) return false;
            return ContainsPrefab(prefab.GetInstanceID());
        }

        public Vector3 minBrushSize
        {
            get
            {
                var min = Vector3.one * float.MaxValue;
                foreach (var item in _items)
                    min = Vector3.Min(min, item.size);
                return min;
            }
        }

        public float minBrushMagnitude
        {
            get
            {
                var min = minBrushSize;
                return Mathf.Min(min.x, min.y, min.z);
            }
        }

        public void UpdateAssetTypes()
        {
            foreach (var item in _items) item.UpdateAssetType();
        }
    }

    [System.Serializable]
    public class BrushCreationSettings
    {
        [SerializeField] private bool _includeSubfolders = true;
        [SerializeField] private bool _createLablesForEachDroppedFolder = false;
        [SerializeField] private bool _addLabelsToDroppedPrefabs = false;
        [SerializeField] private string _labelsCSV = null;
        private string[] _labels = null;
        [SerializeField] private BrushSettings _defaultBrushSettings = new BrushSettings();
        [SerializeField] private ThumbnailSettings _defaultThumbnailSettings = new ThumbnailSettings();

        public bool includeSubfolders
        {
            get => _includeSubfolders;
            set
            {
                if (_includeSubfolders == value) return;
                _includeSubfolders = value;
            }
        }
        public bool createLablesForEachDroppedFolder
        {
            get => _createLablesForEachDroppedFolder;
            set
            {
                if (_createLablesForEachDroppedFolder == value) return;
                _createLablesForEachDroppedFolder = value;
            }
        }
        public bool addLabelsToDroppedPrefabs
        {
            get => _addLabelsToDroppedPrefabs;
            set
            {
                if (_addLabelsToDroppedPrefabs == value) return;
                _addLabelsToDroppedPrefabs = value;
            }
        }

        private void SplitCSV() => _labels = _labelsCSV.Replace(", ", ",").Split(',');

        public string[] labels
        {
            get
            {
                if (_labels == null || (_labels.Length == 0 && _labelsCSV != null && _labelsCSV != string.Empty))
                    SplitCSV();
                return _labels;
            }
        }

        public string labelsCSV
        {
            get => _labelsCSV;
            set
            {
                if (_labelsCSV == value) return;
                if (value == string.Empty)
                {
                    _labelsCSV = string.Empty;
                    _labels = new string[0];
                    return;
                }
                var trimmed = System.Text.RegularExpressions.Regex.Replace(value.Trim(), "[( *, +)]+", ", ");
                if (trimmed.Last() == ' ') trimmed = trimmed.Substring(0, trimmed.Length - 2);
                if (trimmed.First() == ',') trimmed = trimmed.Substring(1);
                if (_labelsCSV == trimmed) return;
                _labelsCSV = trimmed;
                SplitCSV();
            }
        }

        public BrushSettings defaultBrushSettings => _defaultBrushSettings;
        public void FactoryResetDefaultBrushSettings() => _defaultBrushSettings = new BrushSettings();

        public ThumbnailSettings defaultThumbnailSettings => _defaultThumbnailSettings;
        public void FactoryResetDefaultThumbnailSettings() => _defaultThumbnailSettings = new ThumbnailSettings();

        public BrushCreationSettings Clone()
        {
            var clone = new BrushCreationSettings();
            clone.Copy(this);
            return clone;
        }

        public void Copy(BrushCreationSettings other)
        {
            _includeSubfolders = other._includeSubfolders;
            _createLablesForEachDroppedFolder = other._createLablesForEachDroppedFolder;
            _addLabelsToDroppedPrefabs = other._addLabelsToDroppedPrefabs;
            _labelsCSV = other._labelsCSV;
            if (other._labels != null)
            {
                _labels = new string[other._labels.Length];
                System.Array.Copy(other._labels, _labels, other._labels.Length);
            }
            _defaultBrushSettings.Copy(other._defaultBrushSettings);
            _defaultThumbnailSettings.Copy(other._defaultThumbnailSettings);
        }
    }
    public class BrushInputData
    {
        public readonly int index;
        public readonly Rect rect;
        public readonly EventType eventType;
        public readonly bool control;
        public readonly bool shift;
        public readonly float mouseX;
        public BrushInputData(int index, Rect rect, EventType eventType, bool control, bool shift, float mouseX)
        {
            this.index = index;
            this.rect = rect;
            this.eventType = eventType;
            this.control = !shift && control;
            this.shift = shift;
            this.mouseX = mouseX;
        }
    }

    #endregion
    [System.Serializable]
    public class PaletteData
    {
        [SerializeField] private string _version = PWBData.VERSION;
        [SerializeField]
        private System.Collections.Generic.List<MultibrushSettings> _brushes
            = new System.Collections.Generic.List<MultibrushSettings>();
        [SerializeField] private string _name = null;
        [SerializeField] private long _id = -1;
        [SerializeField] BrushCreationSettings _brushCreationSettings = new BrushCreationSettings();
        private string _filePath = null;
        private bool _saving = false;
        public PaletteData(string name, long id) => (_name, _id) = (name, id);

        public string name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                Save();
            }
        }
        public long id => _id;

        public MultibrushSettings[] brushes => _brushes.ToArray();

        public int brushCount => _brushes.Count;

        public BrushCreationSettings brushCreationSettings => _brushCreationSettings;

        public string filePath { get => _filePath; set => _filePath = value; }

        public string version { get => _version; set => _version = value; }
        public bool saving => _saving;
        public static string GetFileNameFromData(PaletteData data) => "PWB_" + data._id.ToString("X") + ".txt";

        public MultibrushSettings GetBrush(int idx)
        {
            if (idx < 0 || idx >= _brushes.Count) return null;
            return _brushes[idx];
        }

        private void SetSpritesThumbnailSettings(MultibrushSettings brush)
        {
            foreach (var item in brush.items)
            {
                if (item.is2DAsset)
                {
                    item.thumbnailSettings.targetEuler = new Vector3(17.5f, 0f, 0f);
                    item.thumbnailSettings.zoom = 1.47f;
                    item.thumbnailSettings.targetOffset = new Vector2(0f, -0.06f);
                }
            }
            brush.rotateToTheSurface = false;
        }

        public void AddBrush(MultibrushSettings brush)
        {
            _brushes.Add(brush);
            SetSpritesThumbnailSettings(brush);
            brush.palette = this;
            PWBCore.staticData.Save();
            Save();
        }

        public void RemoveBrushAt(int idx)
        {
            _brushes.RemoveAt(idx);
            PWBCore.staticData.Save();
            BrushstrokeManager.UpdateBrushstroke();
            Save();
        }

        public void RemoveBrush(MultibrushSettings brush)
        {
            _brushes.Remove(brush);
            PWBCore.staticData.Save();
            BrushstrokeManager.UpdateBrushstroke();
            PrefabPalette.OnChangeRepaint();
            Save();
        }

        public void InsertBrushAt(MultibrushSettings brush, int idx)
        {
            _brushes.Insert(idx, brush);
            SetSpritesThumbnailSettings(brush);
            brush.palette = this;
            PWBCore.staticData.Save();
            Save();
        }

        public void Swap(int fromIdx, int toIdx, ref int[] selection)
            => SelectionUtils.Swap(fromIdx, toIdx, ref selection, _brushes);

        public void AscendingSort()
        {
            _brushes.Sort(delegate (MultibrushSettings x, MultibrushSettings y) { return x.name.CompareTo(y.name); });
            PaletteManager.ClearSelection();
            PWBCore.staticData.Save();
            PrefabPalette.OnChangeRepaint();
        }

        public void DescendingSort()
        {
            _brushes.Sort(delegate (MultibrushSettings x, MultibrushSettings y) { return y.name.CompareTo(x.name); });
            PaletteManager.ClearSelection();
            PWBCore.staticData.Save();
            PrefabPalette.OnChangeRepaint();
        }

        public void DuplicateBrush(int index) => DuplicateBrushAt(index, index);

        public void DuplicateBrushAt(int indexToDuplicate, int at)
        {
            var clone = _brushes[indexToDuplicate].Clone();
            _brushes.Insert(at, clone as MultibrushSettings);
            PWBCore.staticData.Save();
            Save();
        }

        public bool ContainsSceneObject(GameObject obj)
        {
            if (obj == null) return false;
            var outermostPrefab = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(obj);
            if (outermostPrefab == null) return false;
            var prefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(outermostPrefab);
            if (prefab == null) return false;
            return _brushes.Exists(brush => brush.ContainsPrefab(prefab.GetInstanceID()));
        }

        public int FindBrushIdx(GameObject obj)
        {
            if (obj == null) return -1;
            var outermostPrefab = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(obj);
            if (outermostPrefab == null) return -1;
            var prefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(outermostPrefab);
            if (prefab == null) return -1;
            var idx = _brushes.FindIndex(brush => brush.ContainsPrefab(prefab.GetInstanceID()) && brush.itemCount == 1);
            if (idx == -1) idx = _brushes.FindIndex(brush => brush.ContainsPrefab(prefab.GetInstanceID()));
            return idx;
        }

        public bool ContainsBrush(MultibrushSettings brush) => _brushes.Contains(brush);

        public string Save()
        {
            _saving = true;
            if (filePath == null) filePath = PWBCore.staticData.palettesFullDirectory + PaletteData.GetFileNameFromData(this);
            var jsonString = JsonUtility.ToJson(this);
            System.IO.File.WriteAllText(filePath, jsonString);
            UnityEditor.AssetDatabase.Refresh();
            _saving = false;
            return filePath;
        }

        public void Copy(PaletteData other)
        {
            _brushes.Clear();
            _brushes.AddRange(other.brushes.ToArray());
            _name = other.name;
            _brushCreationSettings.Copy(other._brushCreationSettings);
        }
        public void ReloadFromFile()
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(_filePath);
            var relativePath = PWBData.PALETTES_RESOURCE_DIR + fileName;
            var textAsset = Resources.Load<TextAsset>(relativePath);
            if (textAsset == null) return;
            if (string.IsNullOrEmpty(textAsset.text)) return;
            var paletteData = JsonUtility.FromJson<PaletteData>(textAsset.text);
            if (paletteData == null) return;
            Copy(paletteData);
        }
    }

    [System.Serializable]
    public class PaletteManager : ISerializationCallbackReceiver
    {
        private System.Collections.Generic.List<PaletteData> _paletteDataList = new System.Collections.Generic.List<PaletteData>()
        { new PaletteData("Palette", System.DateTime.Now.ToBinary()) };
        public static PaletteData[] paletteData => instance.paletteDataList.ToArray();

        [SerializeField] private int _selectedPaletteIdx = 0;
        [SerializeField] private int _selectedBrushIdx = -1;
        [SerializeField] private bool _showBrushName = false;
        [SerializeField] private bool _viewList = false;
        private System.Collections.Generic.HashSet<int> _idxSelection = new System.Collections.Generic.HashSet<int>();

        public static System.Action OnBrushChanged;
        public static System.Action OnSelectionChanged;
        public static System.Action OnPaletteChanged;

        private bool _pickingBrushes = false;
        private bool _loadPaletteFiles = false;
        public List<PaletteData> paletteDataList
        {
            get
            {
                if (_loadPaletteFiles)
                {
                    _loadPaletteFiles = false;
                    PWBCore.staticData.VersionUpdate();
                    LoadPaletteFiles();
                }
                return _paletteDataList;
            }
        }

        private static PaletteManager _instance = null;

        private PaletteManager() { }
        public static PaletteManager instance
        {
            get
            {
                if (_instance == null) _instance = new PaletteManager();
                return _instance;
            }
        }

        public void LoadPaletteFiles()
        {
            var txtPaths = System.IO.Directory.GetFiles(PWBCore.staticData.palettesFullDirectory, "*.txt");
            if (txtPaths.Length == 0)
            {
                if (_paletteDataList.Count == 0)
                    _paletteDataList = new System.Collections.Generic.List<PaletteData>()
                            { new PaletteData("Palette", System.DateTime.Now.ToBinary()) };
                _paletteDataList[0].filePath = _paletteDataList[0].Save();
            }
            bool clearList = true;
            foreach (var path in txtPaths)
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                var relativePath = PWBData.PALETTES_RESOURCE_DIR + fileName;
                var textAsset = Resources.Load<TextAsset>(relativePath);
                if (textAsset == null) continue;
                if (string.IsNullOrEmpty(textAsset.text)) continue;
                var paletteData = JsonUtility.FromJson<PaletteData>(textAsset.text);
                if (paletteData == null) continue;
                if (clearList)
                {
                    _paletteDataList.Clear();
                    clearList = false;
                }
                paletteData.filePath = path;
                _paletteDataList.Add(paletteData);
            }
        }

        public static void Clear()
        {
            instance.paletteDataList.Clear();
            instance.paletteDataList.Add(new PaletteData("Palette", System.DateTime.Now.ToBinary()));
            instance._selectedPaletteIdx = 0;
            instance._selectedBrushIdx = -1;
            instance._idxSelection.Clear();
            instance._pickingBrushes = false;
        }

        public static bool showBrushName
        {
            get => instance._showBrushName;
            set
            {
                if (instance._showBrushName == value) return;
                instance._showBrushName = value;
                PWBCore.staticData.Save();
            }
        }

        public static bool viewList
        {
            get => instance._viewList;
            set
            {
                if (instance._viewList == value) return;
                instance._viewList = value;
                PWBCore.staticData.Save();
            }
        }
        public static void ClearPaletteList() => instance._paletteDataList.Clear();
        public static void AddPalette(PaletteData palette)
        {
            instance._paletteDataList.Add(palette);
            palette.filePath = PWBCore.staticData.palettesFullDirectory + PaletteData.GetFileNameFromData(palette);
            palette.Save();
        }

        public static void RemovePaletteAt(int paletteIdx)
        {
            var filePath = instance._paletteDataList[paletteIdx].filePath;
            instance._paletteDataList.RemoveAt(paletteIdx);
            System.IO.File.Delete(filePath);
            UnityEditor.AssetDatabase.Refresh();
        }

        public static void SwapPalette(int from, int to)
        {
            if (from == to) return;
            instance.paletteDataList.Insert(to, instance.paletteDataList[from]);
            var removeIdx = from;
            if (from > to) ++removeIdx;
            instance.paletteDataList.RemoveAt(removeIdx);
        }

        public static void SelectNextPalette()
        {
            if (PrefabPalette.instance == null) return;
            if (paletteCount <= 1) return;
            instance._idxSelection.Clear();
            PrefabPalette.instance.SelectPalette((selectedPaletteIdx + 1) % paletteCount);
            selectedBrushIdx = 0;
            AddToSelection(selectedBrushIdx);
            PrefabPalette.instance.FrameSelectedBrush();
            PrefabPalette.RepainWindow();
        }

        public static void SelectPreviousPalette()
        {
            if (PrefabPalette.instance == null) return;
            if (paletteCount <= 1) return;
            instance._idxSelection.Clear();
            PrefabPalette.instance.SelectPalette((selectedPaletteIdx == 0 ? paletteCount : selectedPaletteIdx) - 1);
            selectedBrushIdx = 0;
            AddToSelection(selectedBrushIdx);
            PrefabPalette.instance.FrameSelectedBrush();
            PrefabPalette.RepainWindow();
        }

        public static string[] paletteNames => instance.paletteDataList.Select(p => p.name).ToArray();

        public static int selectedPaletteIdx
        {
            get
            {
                instance._selectedPaletteIdx = Mathf.Clamp(instance._selectedPaletteIdx, 0,
                    Mathf.Max(instance.paletteDataList.Count - 1, 0));
                return instance._selectedPaletteIdx;
            }
            set
            {
                value = Mathf.Max(value, 0);
                if (instance._selectedPaletteIdx == value) return;
                instance._selectedPaletteIdx = value;
                if (OnPaletteChanged != null) OnPaletteChanged();
            }
        }

        public static int selectedBrushIdx
        {
            get => instance._selectedBrushIdx;
            set
            {
                if (instance._selectedBrushIdx == value) return;
                instance._selectedBrushIdx = value;
                if (selectedBrush != null)
                {
                    selectedBrush.UpdateBottomVertices();
                    selectedBrush.UpdateAssetTypes();
                }
                else instance._selectedBrushIdx = -1;
                BrushstrokeManager.UpdateBrushstroke(true);
                if (ToolManager.tool == ToolManager.PaintTool.PIN) PWBIO.ResetPinValues();
                if (OnBrushChanged != null) OnBrushChanged();
            }
        }

        public static bool pickingBrushes
        {
            get => instance._pickingBrushes;
            set
            {
                if (instance._pickingBrushes == value) return;
                instance._pickingBrushes = value;
                if (instance._pickingBrushes)
                {
                    PWBCore.UpdateTempColliders();
                    PWBIO.repaint = true;
                    UnityEditor.SceneView.RepaintAll();
                }
                PrefabPalette.RepainWindow();
            }
        }

        public static void SelectBrush(int idx)
        {
            if (PrefabPalette.instance == null) return;
            if (selectedPalette.brushCount == 0) return;
            if (!PrefabPalette.instance.FilteredBrushListContains(idx)) return;
            instance._idxSelection.Clear();
            selectedBrushIdx = idx;
            if (selectedBrush != null)
            {
                selectedBrush.UpdateBottomVertices();
                selectedBrush.UpdateAssetTypes();
            }
            AddToSelection(selectedBrushIdx);
            PrefabPalette.instance.FrameSelectedBrush();
            PrefabPalette.RepainWindow();
        }

        public static void SelectNextBrush()
        {
            if (PrefabPalette.instance == null) return;
            if (selectedPalette.brushCount <= 1) return;
            instance._idxSelection.Clear();
            int selectedIdx = instance._selectedBrushIdx;
            int count = 0;
            do
            {
                selectedIdx = (selectedIdx + 1) % selectedPalette.brushCount;
                if (++count > selectedPalette.brushCount) return;
            }
            while (!PrefabPalette.instance.FilteredBrushListContains(selectedIdx));
            selectedBrushIdx = selectedIdx;
            if (selectedBrush != null)
            {
                selectedBrush.UpdateBottomVertices();
                selectedBrush.UpdateAssetTypes();
            }
            AddToSelection(selectedBrushIdx);
            PrefabPalette.instance.FrameSelectedBrush();
        }
        public static void SelectPreviousBrush()
        {
            if (PrefabPalette.instance == null) return;
            if (selectedPalette.brushCount <= 1) return;
            instance._idxSelection.Clear();
            int selectedIdx = instance._selectedBrushIdx;
            int count = 0;
            do
            {
                selectedIdx = (selectedIdx == 0 ? selectedPalette.brushCount : selectedIdx) - 1;
                if (++count > selectedPalette.brushCount) return;
            }
            while (!PrefabPalette.instance.FilteredBrushListContains(selectedIdx));
            selectedBrushIdx = selectedIdx;
            if (selectedBrush != null)
            {
                selectedBrush.UpdateBottomVertices();
                selectedBrush.UpdateAssetTypes();
            }
            AddToSelection(selectedBrushIdx);
            PrefabPalette.instance.FrameSelectedBrush();
        }

        public static MultibrushSettings selectedBrush
            => instance._selectedBrushIdx < 0 ? null : selectedPalette.GetBrush(instance._selectedBrushIdx);

        public static PaletteData selectedPalette => instance.paletteDataList[instance._selectedPaletteIdx];

        public static int paletteCount => instance.paletteDataList.Count;

        public static MultibrushSettings GetBrushById(long id)
        {
            foreach (var palette in instance.paletteDataList)
                foreach (var brush in palette.brushes)
                    if (brush.id == id) return brush;
            return null;
        }

        public static int GetBrushIdx(long id)
        {
            var palette = selectedPalette;
            var brushes = palette.brushes;
            for (int i = 0; i < brushes.Length; ++i)
                if (brushes[i].id == id) return i;
            return -1;
        }

        public static int[] idxSelection
        {
            get => instance._idxSelection.ToArray();
            set
            {
                instance._idxSelection = new System.Collections.Generic.HashSet<int>(value);
                if (OnSelectionChanged != null) OnSelectionChanged();
            }
        }
        public static int selectionCount
        {
            get
            {
                if (instance._idxSelection.Count == 0 && instance._selectedBrushIdx > 0 && selectedBrush != null)
                {
                    instance._idxSelection.Add(instance._selectedBrushIdx);
                    if (OnSelectionChanged != null) OnSelectionChanged();
                }
                return instance._idxSelection.Count;
            }
        }
        public static void AddToSelection(int index)
        {
            instance._idxSelection.Add(index);
            if (OnSelectionChanged != null) OnSelectionChanged();
        }
        public static bool SelectionContains(int index) => instance._idxSelection.Contains(index);
        public static void RemoveFromSelection(int index)
        {
            instance._idxSelection.Remove(index);
            if (OnSelectionChanged != null) OnSelectionChanged();
        }
        public static void ClearSelection(bool updateBrushProperties = true)
        {
            instance._selectedBrushIdx = -1;
            instance._idxSelection.Clear();
            if (!updateBrushProperties) return;
            if (OnSelectionChanged != null) OnSelectionChanged();
            BrushProperties.RepaintWindow();
        }

        public static PaletteData GetPalette(MultibrushSettings brush)
        {
            foreach (var palette in instance.paletteDataList)
                if (palette.ContainsBrush(brush)) return palette;
            return null;
        }

        public static PaletteData GetPalette(long id)
        {
            foreach (var palette in instance.paletteDataList)
                if (palette.id == id) return palette;
            return null;
        }
        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() => _loadPaletteFiles = true;

        #region CLIPBOARD
        private static BrushSettings _clipboardSettings = null;
        private static ThumbnailSettings _clipboardThumbnailSettings = null;
        public enum Trit { FALSE, TRUE, SAME }
        private static Trit _clipboardOverwriteThumbnailSettings = Trit.FALSE;
        public static BrushSettings clipboardSetting { get => _clipboardSettings; set => _clipboardSettings = value; }
        public static ThumbnailSettings clipboardThumbnailSettings
        { get => _clipboardThumbnailSettings; set => _clipboardThumbnailSettings = value; }
        public static Trit clipboardOverwriteThumbnailSettings
        { get => _clipboardOverwriteThumbnailSettings; set => _clipboardOverwriteThumbnailSettings = value; }

        #endregion
    }

}
