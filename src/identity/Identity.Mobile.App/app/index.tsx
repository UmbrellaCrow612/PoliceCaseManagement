import { useState, useCallback } from "react";
import { View, Text, FlatList, StyleSheet } from "react-native";
import { Link, useFocusEffect } from "expo-router";
import AsyncStorage from "@react-native-async-storage/async-storage";

interface TotpData {
  issuer: string;
  appName: string;
  userName: string;
  secret: string;
}

export default function TotpListTab() {
  const [totpList, setTotpList] = useState<TotpData[]>([]);

  useFocusEffect(
    useCallback(() => {
      let isActive = true;

      const loadTotp = async () => {
        try {
          const keysString = await AsyncStorage.getItem("totp-keys");
          if (!keysString) {
            setTotpList([]);
            return;
          }

          const keys = JSON.parse(keysString) as string[];
          const totpPromises = keys.map(async (key) => {
            const item = await AsyncStorage.getItem(key);
            return item ? (JSON.parse(item) as TotpData) : null;
          });

          const totps: any = (await Promise.all(totpPromises)).filter(Boolean);
          if (isActive) setTotpList(totps);
        } catch (error) {
          console.error("Error loading TOTPs:", error);
        }
      };

      loadTotp();
      return () => {
        isActive = false;
      };
    }, [])
  );

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Stored TOTP Codes</Text>
      <Link href={"/scan"}>Scan</Link>
      <FlatList
        data={totpList}
        keyExtractor={(item) => `${item.issuer}-${item.userName}`}
        renderItem={({ item }) => (
          <View style={styles.item}>
            <Text>Issuer: {item.issuer}</Text>
            <Text>App Name: {item.appName}</Text>
            <Text>Username: {item.userName}</Text>
            <Text>Secret: {item.secret}</Text>
          </View>
        )}
        ListEmptyComponent={<Text>No TOTP entries found</Text>}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  },
  btn: {
    padding: 2,
    backgroundColor: "black",
  },
  title: {},
  item: {},
});
