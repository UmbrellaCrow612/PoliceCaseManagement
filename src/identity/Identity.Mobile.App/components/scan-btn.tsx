import { useNavigation } from "expo-router";
import { Button, View } from "react-native";

export default function ScanBtn() {
  const n = useNavigation<any>();

  return <Button title="Scan QR Code" onPress={() => n.navigate("scan")} />;
}
