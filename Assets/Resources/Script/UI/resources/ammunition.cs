using UnityEngine;
using Text = UnityEngine.UI.Text;

public class ammunition : MonoBehaviour, IDataPersistence
{
    [Tooltip("��ҩUI����")]
    public Text ammunition_text;

    [Tooltip("ȼ��UI����")]
    public Text fuel_text;

    [Tooltip("��UI����")]
    public Text aluminium_text;

    [Tooltip("�ֲ�UI����")]
    public Text steel_text;

    //��������
    private int ammunitions = 0;
    private int fuel = 0;
    private int aluminium = 0;
    private int steel = 0;

    // Start is called before the first frame update
    private void Start()
    {
        ammunition_text.text = "0";
        fuel_text.text = "0";
        aluminium_text.text = "0";
        steel_text.text = "0";
    }

    public void LoadData(GameData data)
    {
        //��ȡ����
        this.ammunitions = data.ammunition;
        this.fuel = data.fuel;
        this.aluminium = data.aluminium;
        this.steel = data.steel;
    }

    public void SaveData(GameData data)
    {
        //��������
        data.ammunition = this.ammunitions;
        data.fuel = this.fuel;
        data.aluminium = this.aluminium;
        data.steel = this.steel;
    }

    // Update is called once per frame
    private void Update()
    {
        //��ӡ����
        ammunition_text.text = ammunitions.ToString();
        fuel_text.text = fuel.ToString();
        aluminium_text.text = aluminium.ToString();
        steel_text.text = steel.ToString();
    }
}