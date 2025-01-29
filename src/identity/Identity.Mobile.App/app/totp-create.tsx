import { useLocalSearchParams } from "expo-router";
import { Text, View } from "react-native";

export default function TotpCreateTab() {
  const local = useLocalSearchParams<any>();

  return (
    <View style={{ padding: 20 }}>
      <Text style={{ fontSize: 20, marginBottom: 16 }}>
        Creating TOTP with:
      </Text>
      <Text>Issuer: {local.issuer}</Text>
      <Text>App Name: {local.appName}</Text>
      <Text>Username: {local.userName}</Text>
      <Text>Secret: {local.secret}</Text>
    </View>
  );
}
