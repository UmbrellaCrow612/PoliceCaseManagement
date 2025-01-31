import React, { useState, useEffect } from "react";
import { View, StyleSheet, FlatList, Text } from "react-native";
import AsyncStorage from "@react-native-async-storage/async-storage";
import TotpItem from "@/components/totp-item";
import ScanBtn from "@/components/scan-btn";

interface TotpData {
  issuer: string;
  userName: string;
  secret: string;
  appName: string;
}

export default function TotpListTab() {
  const [totpItems, setTotpItems] = useState<TotpData[]>([]);

  useEffect(() => {
    fetchTotpItems();
  }, []);

  const fetchTotpItems = async () => {
    try {
      const keysString = await AsyncStorage.getItem("totp-keys");
      if (!keysString) return;

      const keys = JSON.parse(keysString);

      const items: TotpData[] = await Promise.all(
        keys.map(async (key: string) => {
          const itemString = await AsyncStorage.getItem(key);
          return itemString ? JSON.parse(itemString) : null;
        })
      );

      setTotpItems(items.filter(Boolean));
    } catch (error) {
      console.error("Failed to fetch TOTP items:", error);
    }
  };

  const renderTotpItem = ({ item }: { item: TotpData }) => (
    <TotpItem
      details={{
        appName: item.appName,
        issuer: item.issuer,
        secret: item.secret,
        userName: item.userName,
      }}
    />
  );

  return (
    <View style={styles.container}>
      <View>
        <ScanBtn />
      </View>
      <FlatList
        data={totpItems}
        renderItem={renderTotpItem}
        keyExtractor={(item) => `${item.issuer}-${item.userName}`}
        contentContainerStyle={styles.listContainer}
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>No TOTP items found</Text>
          </View>
        }
        ItemSeparatorComponent={() => <View style={styles.separator} />}
        showsVerticalScrollIndicator={true}
        bounces={true}
        alwaysBounceVertical={true}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f5f5f5",
  },
  listContainer: {
    paddingTop: 16,
    paddingBottom: 24,
    paddingHorizontal: 16,
  },
  separator: {
    height: 12,
    backgroundColor: "transparent",
  },
  emptyContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    marginTop: 50,
  },
  emptyText: {
    fontSize: 16,
    color: "#888",
  },
});
